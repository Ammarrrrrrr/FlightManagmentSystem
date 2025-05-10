-- Flight Management System Example Commands
-- This file demonstrates how to use the stored procedures and queries

-- 1. Using the BookFlight stored procedure
-- Book a flight for passenger_id 1 on flight_id 1 in Business class
CALL BookFlight(1, 1, '1A', 'Business');

-- 2. Using the GetFlightDetails stored procedure
-- Get detailed information about flight_id 1
CALL GetFlightDetails(1);

-- 3. Using the GetPassengerBookings stored procedure
-- Get booking history for passenger_id 1
CALL GetPassengerBookings(1);

-- 4. Using the GetFlightStatistics stored procedure
-- Get statistics for airline_id 1 (EgyptAir)
CALL GetFlightStatistics(1);

-- 5. Using the TransferBooking stored procedure
-- Transfer booking_id 1 to flight_id 2
CALL TransferBooking(1, 2);

-- 6. Example CRUD Operations

-- SELECT: Get all flights from Cairo
SELECT f.*, a.airport_name 
FROM flights f 
JOIN airports a ON f.departure_airport_id = a.airport_id 
WHERE a.city = 'Cairo';

-- INSERT: Add a new aircraft
INSERT INTO aircraft (model, manufacturer, capacity, range_km, max_speed_kmh, airline_id)
VALUES ('Boeing 737 MAX 9', 'Boeing', 193, 6570, 842, 1);

-- UPDATE: Update flight status
UPDATE flights 
SET status = 'Delayed' 
WHERE flight_id = 1;

-- DELETE: Remove a cancelled booking
DELETE FROM bookings 
WHERE status = 'Cancelled';

-- 7. Example JOIN Operations

-- Get all flights with their aircraft and airline details
SELECT 
    f.flight_number,
    a.model as aircraft_model,
    al.airline_name,
    f.departure_time,
    f.arrival_time
FROM flights f
JOIN aircraft a ON f.aircraft_id = a.aircraft_id
JOIN airlines al ON f.airline_id = al.airline_id;

-- Get passenger details with their bookings
SELECT 
    p.first_name,
    p.last_name,
    p.passport_number,
    b.booking_id,
    f.flight_number,
    b.class
FROM passengers p
JOIN bookings b ON p.passenger_id = b.passenger_id
JOIN flights f ON b.flight_id = f.flight_id;

-- 8. Example Aggregate Functions

-- Calculate total revenue by airline
SELECT 
    al.airline_name,
    COUNT(b.booking_id) as total_bookings,
    SUM(b.price_paid) as total_revenue,
    AVG(b.price_paid) as average_price
FROM airlines al
JOIN flights f ON al.airline_id = f.airline_id
JOIN bookings b ON f.flight_id = b.flight_id
GROUP BY al.airline_name;

-- 9. Example Complex Query with Subquery

-- Find flights with above-average booking prices
SELECT 
    f.flight_number,
    b.class,
    b.price_paid,
    (SELECT AVG(price_paid) FROM bookings) as average_price
FROM flights f
JOIN bookings b ON f.flight_id = b.flight_id
WHERE b.price_paid > (SELECT AVG(price_paid) FROM bookings)
ORDER BY b.price_paid DESC;

-- 10. Using Views

-- Check available seats for all flights
SELECT * FROM available_seats;

-- Check revenue for all flights
SELECT * FROM flight_revenue;

-- 11. Example of using indexes
-- These queries will use the indexes we created

-- Search flights by date range
SELECT * FROM flights 
WHERE departure_time BETWEEN '2023-06-15' AND '2023-06-20';

-- Search bookings by date
SELECT * FROM bookings 
WHERE booking_date > '2023-05-20';

-- Search passengers by name
SELECT * FROM passengers 
WHERE first_name LIKE 'A%' AND last_name LIKE 'M%';

-- Search airports by city and country
SELECT * FROM airports 
WHERE city = 'Cairo' AND country = 'Egypt';

-- 12. Example of Transaction Usage
-- This demonstrates how to use transactions for multiple operations

START TRANSACTION;

-- Update flight status
UPDATE flights 
SET status = 'Boarding' 
WHERE flight_id = 1;

-- Update related bookings
UPDATE bookings 
SET status = 'Boarded' 
WHERE flight_id = 1;

-- If everything is successful, commit the transaction
COMMIT;

-- If there was an error, you would use:
-- ROLLBACK;

-- 13. Example of Error Handling
-- This demonstrates how to handle errors in stored procedures

DELIMITER //
CREATE PROCEDURE SafeBookingUpdate(
    IN p_booking_id INT,
    IN p_new_status VARCHAR(20)
)
BEGIN
    DECLARE EXIT HANDLER FOR SQLEXCEPTION
    BEGIN
        ROLLBACK;
        SELECT 'Error occurred while updating booking' as message;
    END;

    START TRANSACTION;
    
    -- Update the booking status
    UPDATE bookings 
    SET status = p_new_status 
    WHERE booking_id = p_booking_id;
    
    -- If no rows were affected, rollback
    IF ROW_COUNT() = 0 THEN
        ROLLBACK;
        SELECT 'No booking found with the specified ID' as message;
    ELSE
        COMMIT;
        SELECT 'Booking updated successfully' as message;
    END IF;
END //
DELIMITER ;

-- Use the error handling procedure
CALL SafeBookingUpdate(1, 'Checked-in'); 