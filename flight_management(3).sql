-- phpMyAdmin SQL Dump
-- version 5.2.1
-- https://www.phpmyadmin.net/
--
-- Host: 127.0.0.1
-- Generation Time: Apr 17, 2025 at 12:00 AM
-- Server version: 10.4.32-MariaDB
-- PHP Version: 8.2.12

SET SQL_MODE = "NO_AUTO_VALUE_ON_ZERO";
START TRANSACTION;
SET time_zone = "+00:00";


/*!40101 SET @OLD_CHARACTER_SET_CLIENT=@@CHARACTER_SET_CLIENT */;
/*!40101 SET @OLD_CHARACTER_SET_RESULTS=@@CHARACTER_SET_RESULTS */;
/*!40101 SET @OLD_COLLATION_CONNECTION=@@COLLATION_CONNECTION */;
/*!40101 SET NAMES utf8mb4 */;

--
-- Database: `flight_management`
--

-- --------------------------------------------------------

--
-- Table structure for table `aircraft`
--

CREATE TABLE `aircraft` (
  `aircraft_id` int(11) NOT NULL,
  `model` varchar(50) NOT NULL,
  `manufacturer` varchar(50) NOT NULL,
  `capacity` int(11) NOT NULL,
  `range_km` int(11) NOT NULL,
  `max_speed_kmh` int(11) DEFAULT NULL,
  `airline_id` int(11) DEFAULT NULL
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_general_ci;

--
-- Dumping data for table `aircraft`
--

INSERT INTO `aircraft` (`aircraft_id`, `model`, `manufacturer`, `capacity`, `range_km`, `max_speed_kmh`, `airline_id`) VALUES
(1, 'Boeing 787-9', 'Boeing', 290, 14140, 956, 1),
(2, 'Airbus A320-200', 'Airbus', 150, 5700, 828, 1),
(3, 'Boeing 777-300ER', 'Boeing', 360, 13649, 905, 2),
(4, 'Airbus A330-300', 'Airbus', 315, 10800, 871, 2),
(5, 'Boeing 777-200LR', 'Boeing', 380, 15740, 905, 3),
(6, 'Airbus A380-800', 'Airbus', 615, 15200, 945, 3),
(7, 'Boeing 737-800', 'Boeing', 189, 5765, 842, 4),
(8, 'Airbus A321neo', 'Airbus', 236, 7400, 828, 5),
(9, 'Embraer E190', 'Embraer', 114, 4445, 890, 6),
(10, 'Boeing 787-8', 'Boeing', 242, 13620, 956, 7),
(11, 'Airbus A350-900', 'Airbus', 325, 15000, 945, 8),
(12, 'Boeing 747-8', 'Boeing', 467, 14815, 988, 9),
(13, 'Airbus A319-100', 'Airbus', 124, 6850, 828, 10),
(14, 'Boeing 737 MAX 8', 'Boeing', 178, 6570, 842, 11),
(15, 'Airbus A340-600', 'Airbus', 380, 14450, 913, 12),
(17, 'ammarplane', 'ammar', 234, 424, 24234, 4);

-- --------------------------------------------------------

--
-- Table structure for table `airlines`
--

CREATE TABLE `airlines` (
  `airline_id` int(11) NOT NULL,
  `airline_name` varchar(100) NOT NULL,
  `iata_code` char(2) NOT NULL,
  `country` varchar(50) NOT NULL,
  `headquarters` varchar(100) NOT NULL,
  `founded_year` int(11) DEFAULT NULL
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_general_ci;

--
-- Dumping data for table `airlines`
--

INSERT INTO `airlines` (`airline_id`, `airline_name`, `iata_code`, `country`, `headquarters`, `founded_year`) VALUES
(1, 'EgyptAir', 'MS', 'Egypt', 'Cairo', 1932),
(2, 'Saudi Arabian Airlines', 'SV', 'Saudi Arabia', 'Jeddah', 1945),
(3, 'Emirates', 'EK', 'UAE', 'Dubai', 1985),
(4, 'Fly Dubai', 'FZ', 'UAE', 'Dubai', 2008),
(5, 'Nesma Airlines', 'NE', 'Saudi Arabia', 'Jeddah', 2010),
(6, 'Air Arabia Egypt', 'E5', 'Egypt', 'Cairo', 2009),
(7, 'Air Cairo', 'SM', 'Egypt', 'Cairo', 2003),
(8, 'Flynas', 'XY', 'Saudi Arabia', 'Riyadh', 2007),
(9, 'Etihad Airways', 'EY', 'UAE', 'Abu Dhabi', 2003),
(10, 'AlMasria Universal Airlines', 'UJ', 'Egypt', 'Cairo', 2007),
(11, 'Alexandria Airlines', 'XH', 'Egypt', 'Alexandria', 2007),
(12, 'Saudia Private Aviation', 'SP', 'Saudi Arabia', 'Jeddah', 2013),
(13, 'Rotana Jet', 'RG', 'UAE', 'Abu Dhabi', 2010),
(14, 'Arab Air Cargo', 'RB', 'Egypt', 'Cairo', 1999),
(15, 'Royal Jet', 'RS', 'UAE', 'Abu Dhabi', 2003);

-- --------------------------------------------------------

--
-- Table structure for table `airports`
--

CREATE TABLE `airports` (
  `airport_id` int(11) NOT NULL,
  `airport_name` varchar(100) NOT NULL,
  `iata_code` char(3) NOT NULL,
  `icao_code` char(4) NOT NULL,
  `city` varchar(50) NOT NULL,
  `country` varchar(50) NOT NULL,
  `latitude` decimal(10,6) DEFAULT NULL,
  `longitude` decimal(10,6) DEFAULT NULL
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_general_ci;

--
-- Dumping data for table `airports`
--

INSERT INTO `airports` (`airport_id`, `airport_name`, `iata_code`, `icao_code`, `city`, `country`, `latitude`, `longitude`) VALUES
(1, 'Cairo International Airport', 'CAI', 'HECA', 'Cairo', 'Egypt', 30.121944, 31.405556),
(2, 'King Abdulaziz International Airport', 'JED', 'OEJN', 'Jeddah', 'Saudi Arabia', 21.679564, 39.156536),
(3, 'Dubai International Airport', 'DXB', 'OMDB', 'Dubai', 'UAE', 25.252778, 55.364444),
(4, 'King Khalid International Airport', 'RUH', 'OERK', 'Riyadh', 'Saudi Arabia', 24.957640, 46.698776),
(5, 'Abu Dhabi International Airport', 'AUH', 'OMAA', 'Abu Dhabi', 'UAE', 24.432972, 54.651138),
(6, 'Sharm El Sheikh International Airport', 'SSH', 'HESH', 'Sharm El Sheikh', 'Egypt', 27.977222, 34.394722),
(7, 'Alexandria International Airport', 'ALY', 'HEAX', 'Alexandria', 'Egypt', 31.183903, 29.948889),
(8, 'Prince Mohammad bin Abdulaziz Airport', 'MED', 'OEMA', 'Medina', 'Saudi Arabia', 24.553422, 39.705061),
(9, 'Sharjah International Airport', 'SHJ', 'OMSJ', 'Sharjah', 'UAE', 25.328575, 55.517150),
(10, 'Hurghada International Airport', 'HRG', 'HEGN', 'Hurghada', 'Egypt', 27.178317, 33.799436),
(11, 'King Fahd International Airport', 'DMM', 'OEDF', 'Dammam', 'Saudi Arabia', 26.471161, 49.797890),
(12, 'Al Maktoum International Airport', 'DWC', 'OMDW', 'Dubai', 'UAE', 24.896356, 55.161389),
(13, 'Borg El Arab Airport', 'HBE', 'HEBA', 'Alexandria', 'Egypt', 30.917669, 29.696408),
(14, 'Taif Regional Airport', 'TIF', 'OETF', 'Taif', 'Saudi Arabia', 21.483418, 40.544334),
(15, 'Ras Al Khaimah International Airport', 'RKT', 'OMRK', 'Ras Al Khaimah', 'UAE', 25.613483, 55.938817);

-- --------------------------------------------------------

--
-- Table structure for table `airport_crew`
--

CREATE TABLE `airport_crew` (
  `crew_id` int(11) NOT NULL,
  `first_name` varchar(50) DEFAULT NULL,
  `last_name` varchar(50) DEFAULT NULL,
  `role` varchar(50) DEFAULT NULL,
  `phone_number` varchar(20) DEFAULT NULL,
  `email` varchar(100) DEFAULT NULL,
  `username` varchar(50) DEFAULT NULL,
  `password_hash` varchar(255) DEFAULT NULL,
  `created_at` timestamp NOT NULL DEFAULT current_timestamp()
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_general_ci;

--
-- Dumping data for table `airport_crew`
--

INSERT INTO `airport_crew` (`crew_id`, `first_name`, `last_name`, `role`, `phone_number`, `email`, `username`, `password_hash`, `created_at`) VALUES
(1, 'John', 'Doe', 'Ground Supervisor', '+1234567890', 'john.doe@example.com', 'johndoe', '$2y$10$ExampleHashedPassword1', '2025-04-16 17:40:52'),
(2, 'Jane', 'Smith', 'Maintenance Chief', '+1987654321', 'jane.smith@example.com', 'janesmith', '$2y$10$ExampleHashedPassword2', '2025-04-16 17:40:52'),
(3, 'Carlos', 'Martinez', 'Operations Manager', '+1098765432', 'carlos.martinez@example.com', 'cmartinez', '$2y$10$ExampleHashedPassword3', '2025-04-16 17:40:52'),
(4, 'Emily', 'Nguyen', 'Security Officer', '+1029384756', 'emily.nguyen@example.com', 'emilynguyen', '$2y$10$ExampleHashedPassword4', '2025-04-16 17:40:52'),
(8, 'System', 'Admin', 'Administrator', '+10000000000', 'admin@fms.local', 'admincrew', '$2a$11$a9MFdmiBxo4Cs8m6y.07Yeyqu4Gslgw6k3/I31yajNRvxc5Id6nFm', '2025-04-16 18:12:04');

-- --------------------------------------------------------

--
-- Table structure for table `bookings`
--

CREATE TABLE `bookings` (
  `booking_id` int(11) NOT NULL,
  `flight_id` int(11) NOT NULL,
  `passenger_id` int(11) NOT NULL,
  `booking_date` datetime NOT NULL DEFAULT current_timestamp(),
  `seat_number` varchar(10) DEFAULT NULL,
  `class` enum('Economy','Business','First') NOT NULL,
  `price_paid` decimal(10,2) NOT NULL,
  `status` enum('Confirmed','Cancelled','Checked-in','Boarded') DEFAULT 'Confirmed'
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_general_ci;

--
-- Dumping data for table `bookings`
--

INSERT INTO `bookings` (`booking_id`, `flight_id`, `passenger_id`, `booking_date`, `seat_number`, `class`, `price_paid`, `status`) VALUES
(1, 1, 1, '2023-05-20 10:15:00', '12A', 'Economy', 2500.00, 'Confirmed'),
(2, 1, 2, '2023-05-21 11:30:00', '1B', 'Business', 6000.00, 'Confirmed'),
(3, 2, 3, '2023-05-22 09:45:00', '8C', 'Economy', 2200.00, 'Confirmed'),
(4, 2, 4, '2023-05-23 14:20:00', '2A', 'First', 9500.00, 'Confirmed'),
(5, 3, 5, '2023-05-24 16:30:00', '15D', 'Economy', 1800.00, 'Confirmed'),
(6, 3, 6, '2023-05-25 10:10:00', '3C', 'Business', 4500.00, 'Confirmed'),
(7, 4, 7, '2023-05-26 12:45:00', '10B', 'Economy', 800.00, 'Confirmed'),
(8, 4, 8, '2023-05-27 15:20:00', '1D', 'First', 4000.00, 'Confirmed'),
(9, 5, 9, '2023-05-28 08:30:00', '7A', 'Economy', 1500.00, 'Confirmed'),
(10, 5, 10, '2023-05-29 17:40:00', '4B', 'Business', 3500.00, 'Confirmed'),
(11, 6, 11, '2023-05-30 13:15:00', '9C', 'Economy', 500.00, 'Confirmed'),
(12, 6, 12, '2023-05-31 11:25:00', '2D', 'First', 2500.00, 'Confirmed'),
(13, 7, 13, '2023-06-01 14:50:00', '14A', 'Economy', 2000.00, 'Confirmed'),
(14, 7, 14, '2023-06-02 10:05:00', '3A', 'Business', 5000.00, 'Confirmed'),
(15, 8, 15, '2023-06-03 09:30:00', '5B', 'Economy', 1200.00, 'Confirmed');

-- --------------------------------------------------------

--
-- Table structure for table `flights`
--

CREATE TABLE `flights` (
  `flight_id` int(11) NOT NULL,
  `flight_number` varchar(10) NOT NULL,
  `departure_airport_id` int(11) NOT NULL,
  `arrival_airport_id` int(11) NOT NULL,
  `departure_time` datetime NOT NULL,
  `arrival_time` datetime NOT NULL,
  `aircraft_id` int(11) NOT NULL,
  `airline_id` int(11) NOT NULL,
  `status` enum('Scheduled','Boarding','In Air','Landed','Cancelled','Delayed') DEFAULT 'Scheduled',
  `price_economy` decimal(10,2) NOT NULL,
  `price_business` decimal(10,2) NOT NULL,
  `price_first` decimal(10,2) NOT NULL
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_general_ci;

--
-- Dumping data for table `flights`
--

INSERT INTO `flights` (`flight_id`, `flight_number`, `departure_airport_id`, `arrival_airport_id`, `departure_time`, `arrival_time`, `aircraft_id`, `airline_id`, `status`, `price_economy`, `price_business`, `price_first`) VALUES
(1, 'MS123', 1, 3, '2023-06-15 08:00:00', '2023-06-15 10:30:00', 1, 1, 'Scheduled', 2500.00, 6000.00, 10000.00),
(2, 'SV456', 2, 1, '2023-06-15 09:30:00', '2023-06-15 11:45:00', 3, 2, 'Scheduled', 2200.00, 5500.00, 9500.00),
(3, 'EK789', 3, 2, '2023-06-15 11:00:00', '2023-06-15 12:30:00', 5, 3, 'Scheduled', 1800.00, 4500.00, 8500.00),
(4, 'FZ101', 3, 5, '2023-06-16 07:00:00', '2023-06-16 08:00:00', 7, 4, 'Scheduled', 800.00, 2000.00, 4000.00),
(5, 'NE202', 4, 6, '2023-06-16 10:00:00', '2023-06-16 12:00:00', 8, 5, 'Scheduled', 1500.00, 3500.00, 7000.00),
(6, 'E5303', 1, 7, '2023-06-16 14:00:00', '2023-06-16 15:00:00', 9, 6, 'Scheduled', 500.00, 1200.00, 2500.00),
(7, 'SM404', 7, 3, '2023-06-17 16:00:00', '2023-06-17 18:30:00', 10, 7, 'Scheduled', 2000.00, 5000.00, 9000.00),
(8, 'XY505', 4, 8, '2023-06-17 18:00:00', '2023-06-17 19:30:00', 11, 8, 'Scheduled', 1200.00, 3000.00, 6000.00),
(9, 'EY606', 5, 9, '2023-06-18 08:00:00', '2023-06-18 09:00:00', 12, 9, 'Scheduled', 700.00, 1800.00, 3500.00),
(10, 'UJ707', 6, 10, '2023-06-18 12:00:00', '2023-06-18 13:30:00', 13, 10, 'Scheduled', 900.00, 2200.00, 4500.00),
(11, 'XH808', 7, 11, '2023-06-19 09:00:00', '2023-06-19 11:30:00', 14, 11, 'Scheduled', 1600.00, 4000.00, 7500.00),
(12, 'SP909', 8, 12, '2023-06-19 15:00:00', '2023-06-19 17:00:00', 15, 12, 'Scheduled', 1900.00, 4800.00, 8500.00),
(13, 'RG101', 9, 13, '2023-06-20 10:00:00', '2023-06-20 11:30:00', 1, 13, 'Scheduled', 1100.00, 2800.00, 5500.00),
(14, 'RB202', 10, 14, '2023-06-20 14:00:00', '2023-06-20 16:00:00', 2, 14, 'Scheduled', 1300.00, 3200.00, 6500.00),
(15, 'RS303', 12, 15, '2023-06-21 17:00:00', '2023-06-21 18:30:00', 3, 15, 'Scheduled', 1400.00, 3500.00, 7000.00);

-- --------------------------------------------------------

--
-- Table structure for table `passengers`
--

CREATE TABLE `passengers` (
  `passenger_id` int(11) NOT NULL,
  `first_name` varchar(50) NOT NULL,
  `last_name` varchar(50) NOT NULL,
  `passport_number` varchar(20) NOT NULL,
  `nationality` varchar(50) NOT NULL,
  `date_of_birth` date NOT NULL,
  `gender` enum('Male','Female','Other') DEFAULT NULL
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_general_ci;

--
-- Dumping data for table `passengers`
--

INSERT INTO `passengers` (`passenger_id`, `first_name`, `last_name`, `passport_number`, `nationality`, `date_of_birth`, `gender`) VALUES
(1, 'Ahmed', 'Mohamed', 'EG1234567', 'Egyptian', '1985-05-15', 'Male'),
(2, 'Fatima', 'Ali', 'SA9876543', 'Saudi', '1990-08-22', 'Female'),
(3, 'Youssef', 'Hassan', 'EG7654321', 'Egyptian', '1978-11-30', 'Male'),
(4, 'Layla', 'Abdullah', 'SA2345678', 'Saudi', '1995-02-18', 'Female'),
(5, 'Mohamed', 'Ibrahim', 'EG3456789', 'Egyptian', '1982-07-25', 'Male'),
(6, 'Aisha', 'Omar', 'SA4567890', 'Saudi', '1988-04-12', 'Female'),
(7, 'Mahmoud', 'Khalid', 'EG5678901', 'Egyptian', '1992-09-05', 'Male'),
(8, 'Noura', 'Faisal', 'SA6789012', 'Saudi', '1980-12-08', 'Female'),
(9, 'Hassan', 'Samir', 'EG6789012', 'Egyptian', '1975-03-20', 'Male'),
(10, 'Sarah', 'Khalid', 'SA7890123', 'Saudi', '1993-06-14', 'Female'),
(11, 'Ali', 'Mahmoud', 'EG7890123', 'Egyptian', '1987-01-30', 'Male'),
(12, 'Mariam', 'Ahmed', 'SA8901234', 'Saudi', '1998-10-22', 'Female'),
(13, 'Omar', 'Youssef', 'EG8901234', 'Egyptian', '1983-08-15', 'Male'),
(14, 'Huda', 'Sultan', 'SA9012345', 'Saudi', '1979-05-28', 'Female'),
(15, 'Amira', 'Waleed', 'EG9012345', 'Egyptian', '1991-07-10', 'Female');

--
-- Indexes for dumped tables
--

--
-- Indexes for table `aircraft`
--
ALTER TABLE `aircraft`
  ADD PRIMARY KEY (`aircraft_id`),
  ADD KEY `airline_id` (`airline_id`);

--
-- Indexes for table `airlines`
--
ALTER TABLE `airlines`
  ADD PRIMARY KEY (`airline_id`),
  ADD UNIQUE KEY `iata_code` (`iata_code`);

--
-- Indexes for table `airports`
--
ALTER TABLE `airports`
  ADD PRIMARY KEY (`airport_id`),
  ADD UNIQUE KEY `iata_code` (`iata_code`),
  ADD UNIQUE KEY `icao_code` (`icao_code`);

--
-- Indexes for table `airport_crew`
--
ALTER TABLE `airport_crew`
  ADD PRIMARY KEY (`crew_id`),
  ADD UNIQUE KEY `email` (`email`),
  ADD UNIQUE KEY `username` (`username`);

--
-- Indexes for table `bookings`
--
ALTER TABLE `bookings`
  ADD PRIMARY KEY (`booking_id`),
  ADD KEY `flight_id` (`flight_id`),
  ADD KEY `passenger_id` (`passenger_id`);

--
-- Indexes for table `flights`
--
ALTER TABLE `flights`
  ADD PRIMARY KEY (`flight_id`),
  ADD KEY `departure_airport_id` (`departure_airport_id`),
  ADD KEY `arrival_airport_id` (`arrival_airport_id`),
  ADD KEY `aircraft_id` (`aircraft_id`),
  ADD KEY `airline_id` (`airline_id`);

--
-- Indexes for table `passengers`
--
ALTER TABLE `passengers`
  ADD PRIMARY KEY (`passenger_id`),
  ADD UNIQUE KEY `passport_number` (`passport_number`);

--
-- AUTO_INCREMENT for dumped tables
--

--
-- AUTO_INCREMENT for table `aircraft`
--
ALTER TABLE `aircraft`
  MODIFY `aircraft_id` int(11) NOT NULL AUTO_INCREMENT, AUTO_INCREMENT=18;

--
-- AUTO_INCREMENT for table `airlines`
--
ALTER TABLE `airlines`
  MODIFY `airline_id` int(11) NOT NULL AUTO_INCREMENT, AUTO_INCREMENT=16;

--
-- AUTO_INCREMENT for table `airports`
--
ALTER TABLE `airports`
  MODIFY `airport_id` int(11) NOT NULL AUTO_INCREMENT, AUTO_INCREMENT=16;

--
-- AUTO_INCREMENT for table `airport_crew`
--
ALTER TABLE `airport_crew`
  MODIFY `crew_id` int(11) NOT NULL AUTO_INCREMENT, AUTO_INCREMENT=9;

--
-- AUTO_INCREMENT for table `bookings`
--
ALTER TABLE `bookings`
  MODIFY `booking_id` int(11) NOT NULL AUTO_INCREMENT, AUTO_INCREMENT=16;

--
-- AUTO_INCREMENT for table `flights`
--
ALTER TABLE `flights`
  MODIFY `flight_id` int(11) NOT NULL AUTO_INCREMENT, AUTO_INCREMENT=16;

--
-- AUTO_INCREMENT for table `passengers`
--
ALTER TABLE `passengers`
  MODIFY `passenger_id` int(11) NOT NULL AUTO_INCREMENT, AUTO_INCREMENT=16;

--
-- Constraints for dumped tables
--

--
-- Constraints for table `aircraft`
--
ALTER TABLE `aircraft`
  ADD CONSTRAINT `aircraft_ibfk_1` FOREIGN KEY (`airline_id`) REFERENCES `airlines` (`airline_id`);

--
-- Constraints for table `bookings`
--
ALTER TABLE `bookings`
  ADD CONSTRAINT `bookings_ibfk_1` FOREIGN KEY (`flight_id`) REFERENCES `flights` (`flight_id`),
  ADD CONSTRAINT `bookings_ibfk_2` FOREIGN KEY (`passenger_id`) REFERENCES `passengers` (`passenger_id`);

--
-- Constraints for table `flights`
--
ALTER TABLE `flights`
  ADD CONSTRAINT `flights_ibfk_1` FOREIGN KEY (`departure_airport_id`) REFERENCES `airports` (`airport_id`),
  ADD CONSTRAINT `flights_ibfk_2` FOREIGN KEY (`arrival_airport_id`) REFERENCES `airports` (`airport_id`),
  ADD CONSTRAINT `flights_ibfk_3` FOREIGN KEY (`aircraft_id`) REFERENCES `aircraft` (`aircraft_id`),
  ADD CONSTRAINT `flights_ibfk_4` FOREIGN KEY (`airline_id`) REFERENCES `airlines` (`airline_id`);
COMMIT;

/*!40101 SET CHARACTER_SET_CLIENT=@OLD_CHARACTER_SET_CLIENT */;
/*!40101 SET CHARACTER_SET_RESULTS=@OLD_CHARACTER_SET_RESULTS */;
/*!40101 SET COLLATION_CONNECTION=@OLD_COLLATION_CONNECTION */;
