-- phpMyAdmin SQL Dump
-- version 4.6.1
-- http://www.phpmyadmin.net
--
-- Host: localhost:3306
-- Generation Time: Oct 20, 2016 at 10:43 AM
-- Server version: 5.6.30
-- PHP Version: 5.6.22

SET SQL_MODE = "NO_AUTO_VALUE_ON_ZERO";
SET time_zone = "+00:00";


/*!40101 SET @OLD_CHARACTER_SET_CLIENT=@@CHARACTER_SET_CLIENT */;
/*!40101 SET @OLD_CHARACTER_SET_RESULTS=@@CHARACTER_SET_RESULTS */;
/*!40101 SET @OLD_COLLATION_CONNECTION=@@COLLATION_CONNECTION */;
/*!40101 SET NAMES utf8mb4 */;

--
-- Database: `iinallco_data`
--

-- --------------------------------------------------------

--
-- Table structure for table `alias`
--

CREATE TABLE `alias` (
  `love_id` int(11) NOT NULL,
  `alias` int(11) NOT NULL
) ENGINE=InnoDB DEFAULT CHARSET=utf8;

-- --------------------------------------------------------

--
-- Table structure for table `body`
--

CREATE TABLE `body` (
  `id` int(11) NOT NULL,
  `position` int(11) NOT NULL,
  `love_id` int(11) NOT NULL,
  `light_id` int(11) NOT NULL
) ENGINE=InnoDB DEFAULT CHARSET=utf8;

-- --------------------------------------------------------

--
-- Table structure for table `data`
--

CREATE TABLE `data` (
  `id` int(11) NOT NULL,
  `key` varchar(128) NOT NULL,
  `value` text NOT NULL
) ENGINE=InnoDB DEFAULT CHARSET=utf8;

-- --------------------------------------------------------

--
-- Table structure for table `edit_regex`
--

CREATE TABLE `edit_regex` (
  `id` int(11) NOT NULL,
  `regex` varchar(512) COLLATE utf8_unicode_ci NOT NULL
) ENGINE=InnoDB DEFAULT CHARSET=utf8 COLLATE=utf8_unicode_ci;

-- --------------------------------------------------------

--
-- Table structure for table `light`
--

CREATE TABLE `light` (
  `id` int(11) NOT NULL,
  `text` text NOT NULL
) ENGINE=InnoDB DEFAULT CHARSET=utf8;

-- --------------------------------------------------------

--
-- Table structure for table `love`
--

CREATE TABLE `love` (
  `id` int(11) NOT NULL,
  `light` varchar(256) NOT NULL
) ENGINE=InnoDB DEFAULT CHARSET=utf8;

-- --------------------------------------------------------

--
-- Table structure for table `peace`
--

CREATE TABLE `peace` (
  `love_id` int(11) NOT NULL,
  `light_id` int(11) NOT NULL,
  `sequence` int(11) NOT NULL
) ENGINE=InnoDB DEFAULT CHARSET=utf8;

-- --------------------------------------------------------

--
-- Table structure for table `truth`
--

CREATE TABLE `truth` (
  `id` int(11) NOT NULL,
  `love_id` int(11) NOT NULL,
  `truth_id` int(11) NOT NULL,
  `sequence` int(11) NOT NULL
) ENGINE=InnoDB DEFAULT CHARSET=utf8;

--
-- Indexes for dumped tables
--

--
-- Indexes for table `alias`
--
ALTER TABLE `alias`
  ADD PRIMARY KEY (`love_id`),
  ADD UNIQUE KEY `love_id` (`love_id`,`alias`),
  ADD KEY `alias_alias` (`alias`);

--
-- Indexes for table `body`
--
ALTER TABLE `body`
  ADD PRIMARY KEY (`id`);

--
-- Indexes for table `data`
--
ALTER TABLE `data`
  ADD PRIMARY KEY (`id`);

--
-- Indexes for table `light`
--
ALTER TABLE `light`
  ADD PRIMARY KEY (`id`);

--
-- Indexes for table `love`
--
ALTER TABLE `love`
  ADD PRIMARY KEY (`id`),
  ADD UNIQUE KEY `id` (`id`);

--
-- Indexes for table `peace`
--
ALTER TABLE `peace`
  ADD PRIMARY KEY (`love_id`,`light_id`),
  ADD UNIQUE KEY `love_id` (`love_id`,`light_id`),
  ADD KEY `peace_light` (`light_id`);

--
-- Indexes for table `truth`
--
ALTER TABLE `truth`
  ADD PRIMARY KEY (`id`),
  ADD UNIQUE KEY `love_id` (`love_id`,`truth_id`,`sequence`),
  ADD KEY `truth_truth` (`truth_id`);

--
-- AUTO_INCREMENT for dumped tables
--

--
-- AUTO_INCREMENT for table `body`
--
ALTER TABLE `body`
  MODIFY `id` int(11) NOT NULL AUTO_INCREMENT, AUTO_INCREMENT=57;
--
-- AUTO_INCREMENT for table `data`
--
ALTER TABLE `data`
  MODIFY `id` int(11) NOT NULL AUTO_INCREMENT, AUTO_INCREMENT=2;
--
-- AUTO_INCREMENT for table `light`
--
ALTER TABLE `light`
  MODIFY `id` int(11) NOT NULL AUTO_INCREMENT, AUTO_INCREMENT=206;
--
-- AUTO_INCREMENT for table `love`
--
ALTER TABLE `love`
  MODIFY `id` int(11) NOT NULL AUTO_INCREMENT, AUTO_INCREMENT=226;
--
-- AUTO_INCREMENT for table `truth`
--
ALTER TABLE `truth`
  MODIFY `id` int(11) NOT NULL AUTO_INCREMENT, AUTO_INCREMENT=224;
--
-- Constraints for dumped tables
--

--
-- Constraints for table `alias`
--
ALTER TABLE `alias`
  ADD CONSTRAINT `alias_alias` FOREIGN KEY (`alias`) REFERENCES `love` (`id`),
  ADD CONSTRAINT `alias_love` FOREIGN KEY (`love_id`) REFERENCES `love` (`id`);

--
-- Constraints for table `peace`
--
ALTER TABLE `peace`
  ADD CONSTRAINT `peace_light` FOREIGN KEY (`light_id`) REFERENCES `light` (`id`),
  ADD CONSTRAINT `peace_love` FOREIGN KEY (`love_id`) REFERENCES `love` (`id`);

--
-- Constraints for table `truth`
--
ALTER TABLE `truth`
  ADD CONSTRAINT `truth_love` FOREIGN KEY (`love_id`) REFERENCES `love` (`id`),
  ADD CONSTRAINT `truth_truth` FOREIGN KEY (`truth_id`) REFERENCES `love` (`id`);

/*!40101 SET CHARACTER_SET_CLIENT=@OLD_CHARACTER_SET_CLIENT */;
/*!40101 SET CHARACTER_SET_RESULTS=@OLD_CHARACTER_SET_RESULTS */;
/*!40101 SET COLLATION_CONNECTION=@OLD_COLLATION_CONNECTION */;
