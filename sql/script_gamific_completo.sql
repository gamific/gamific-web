-- MySQL Script generated by MySQL Workbench
-- Tue Jan 17 09:57:38 2017
-- Model: New Model    Version: 1.0
-- MySQL Workbench Forward Engineering

SET @OLD_UNIQUE_CHECKS=@@UNIQUE_CHECKS, UNIQUE_CHECKS=0;
SET @OLD_FOREIGN_KEY_CHECKS=@@FOREIGN_KEY_CHECKS, FOREIGN_KEY_CHECKS=0;
SET @OLD_SQL_MODE=@@SQL_MODE, SQL_MODE='TRADITIONAL,ALLOW_INVALID_DATES';

-- -----------------------------------------------------
-- Schema mydb
-- -----------------------------------------------------
-- -----------------------------------------------------
-- Schema GAMIFICPRDNEW
-- -----------------------------------------------------
DROP SCHEMA IF EXISTS `GAMIFICPRDNEW` ;

-- -----------------------------------------------------
-- Schema GAMIFICPRDNEW
-- -----------------------------------------------------
CREATE SCHEMA IF NOT EXISTS `GAMIFICPRDNEW` DEFAULT CHARACTER SET latin1 ;
USE `GAMIFICPRDNEW` ;

-- -----------------------------------------------------
-- Table `GAMIFICPRDNEW`.`Account_UserAccount`
-- -----------------------------------------------------
DROP TABLE IF EXISTS `GAMIFICPRDNEW`.`Account_UserAccount` ;

CREATE TABLE IF NOT EXISTS `GAMIFICPRDNEW`.`Account_UserAccount` (
  `Id` INT(11) NOT NULL AUTO_INCREMENT,
  `PasswordHash` LONGTEXT NULL DEFAULT NULL,
  `SecurityStamp` LONGTEXT NULL DEFAULT NULL,
  `LockoutDate` DATETIME NULL DEFAULT NULL,
  `AccessFailedCount` INT(11) NOT NULL,
  `UserName` VARCHAR(256) NOT NULL,
  `LastUpdate` DATETIME NOT NULL,
  `Status` TINYINT(4) NOT NULL,
  `lastLogin` DATETIME NULL DEFAULT NULL,
  `tokenMobile` VARCHAR(1000) NULL DEFAULT NULL,
  `device` VARCHAR(100) NULL DEFAULT NULL,
  PRIMARY KEY (`Id`),
  INDEX `UserName_index` (`UserName` ASC))
ENGINE = InnoDB
AUTO_INCREMENT = 52
DEFAULT CHARACTER SET = latin1;


-- -----------------------------------------------------
-- Table `GAMIFICPRDNEW`.`Account_UserProfile`
-- -----------------------------------------------------
DROP TABLE IF EXISTS `GAMIFICPRDNEW`.`Account_UserProfile` ;

CREATE TABLE IF NOT EXISTS `GAMIFICPRDNEW`.`Account_UserProfile` (
  `Id` INT(11) NOT NULL,
  `Name` VARCHAR(100) NOT NULL,
  `Email` VARCHAR(100) NOT NULL,
  `CPF` VARCHAR(20) NULL DEFAULT NULL,
  `Phone` VARCHAR(20) NOT NULL,
  `LastUpdate` DATETIME NOT NULL,
  PRIMARY KEY (`Id`))
ENGINE = InnoDB
DEFAULT CHARACTER SET = latin1;


-- -----------------------------------------------------
-- Table `GAMIFICPRDNEW`.`Account_UserRole`
-- -----------------------------------------------------
DROP TABLE IF EXISTS `GAMIFICPRDNEW`.`Account_UserRole` ;

CREATE TABLE IF NOT EXISTS `GAMIFICPRDNEW`.`Account_UserRole` (
  `UserId` INT(11) NOT NULL,
  `Role` INT(11) NOT NULL,
  PRIMARY KEY (`UserId`, `Role`),
  INDEX `Role_FK_idx` (`Role` ASC),
  CONSTRAINT `UserRole_Account_FK`
    FOREIGN KEY (`UserId`)
    REFERENCES `GAMIFICPRDNEW`.`Account_UserAccount` (`Id`)
    ON DELETE NO ACTION
    ON UPDATE NO ACTION)
ENGINE = InnoDB
DEFAULT CHARACTER SET = latin1;


-- -----------------------------------------------------
-- Table `GAMIFICPRDNEW`.`Firm_Data`
-- -----------------------------------------------------
DROP TABLE IF EXISTS `GAMIFICPRDNEW`.`Firm_Data` ;

CREATE TABLE IF NOT EXISTS `GAMIFICPRDNEW`.`Firm_Data` (
  `Id` INT(11) NOT NULL AUTO_INCREMENT,
  `FirmName` VARCHAR(100) NOT NULL,
  `CompanyName` VARCHAR(100) NOT NULL,
  `CNPJ` VARCHAR(100) NOT NULL,
  `LogoId` INT(11) NULL DEFAULT '0',
  `Adress` VARCHAR(100) NOT NULL,
  `Neighborhood` VARCHAR(100) NOT NULL,
  `City` VARCHAR(100) NOT NULL,
  `UpdatedBy` INT(11) NOT NULL,
  `Status` TINYINT(4) NOT NULL,
  `LastUpdate` DATETIME NOT NULL,
  `Phone` VARCHAR(30) NOT NULL,
  `ExternalId` VARCHAR(25) NOT NULL,
  PRIMARY KEY (`Id`))
ENGINE = InnoDB
AUTO_INCREMENT = 8
DEFAULT CHARACTER SET = latin1;


-- -----------------------------------------------------
-- Table `GAMIFICPRDNEW`.`Firm_Worker_Type`
-- -----------------------------------------------------
DROP TABLE IF EXISTS `GAMIFICPRDNEW`.`Firm_Worker_Type` ;

CREATE TABLE IF NOT EXISTS `GAMIFICPRDNEW`.`Firm_Worker_Type` (
  `Id` INT(11) NOT NULL AUTO_INCREMENT,
  `TypeName` VARCHAR(50) NOT NULL,
  `ProfileName` INT(11) NOT NULL,
  `FirmId` INT(11) NOT NULL,
  `UpdatedBy` INT(11) NOT NULL,
  `Status` TINYINT(4) NOT NULL,
  `LastUpdate` DATETIME NOT NULL,
  `ExternalFirmId` VARCHAR(25) NULL DEFAULT NULL,
  PRIMARY KEY (`Id`),
  INDEX `FIRM_ID_PROFILE` (`FirmId` ASC),
  CONSTRAINT `FIRM_ID_PROFILE`
    FOREIGN KEY (`FirmId`)
    REFERENCES `GAMIFICPRDNEW`.`Firm_Data` (`Id`)
    ON DELETE NO ACTION
    ON UPDATE NO ACTION)
ENGINE = InnoDB
AUTO_INCREMENT = 22
DEFAULT CHARACTER SET = latin1;


-- -----------------------------------------------------
-- Table `GAMIFICPRDNEW`.`Firm_Worker`
-- -----------------------------------------------------
DROP TABLE IF EXISTS `GAMIFICPRDNEW`.`Firm_Worker` ;

CREATE TABLE IF NOT EXISTS `GAMIFICPRDNEW`.`Firm_Worker` (
  `Id` INT(11) NOT NULL AUTO_INCREMENT,
  `WorkerTypeId` INT(11) NOT NULL,
  `UserId` INT(11) NOT NULL,
  `FirmId` INT(11) NOT NULL,
  `UpdatedBy` INT(11) NOT NULL,
  `LogoId` INT(11) NULL DEFAULT '0',
  `Status` TINYINT(4) NOT NULL,
  `LastUpdate` DATETIME NOT NULL,
  `ExternalId` VARCHAR(25) NULL DEFAULT NULL,
  `ExternalFirmId` VARCHAR(25) NULL DEFAULT NULL,
  PRIMARY KEY (`Id`),
  UNIQUE INDEX `IdExterno` (`ExternalId` ASC),
  INDEX `FIRM_ID_WORKER` (`FirmId` ASC),
  INDEX `USER_ID_WORKER` (`UserId` ASC),
  INDEX `WORKER_TYPE_ID_WORKER` (`WorkerTypeId` ASC),
  CONSTRAINT `FIRM_ID_WORKER`
    FOREIGN KEY (`FirmId`)
    REFERENCES `GAMIFICPRDNEW`.`Firm_Data` (`Id`)
    ON DELETE NO ACTION
    ON UPDATE NO ACTION,
  CONSTRAINT `USER_ID_WORKER`
    FOREIGN KEY (`UserId`)
    REFERENCES `GAMIFICPRDNEW`.`Account_UserAccount` (`Id`)
    ON DELETE NO ACTION
    ON UPDATE NO ACTION,
  CONSTRAINT `WORKER_TYPE_ID_WORKER`
    FOREIGN KEY (`WorkerTypeId`)
    REFERENCES `GAMIFICPRDNEW`.`Firm_Worker_Type` (`Id`)
    ON DELETE NO ACTION
    ON UPDATE NO ACTION)
ENGINE = InnoDB
AUTO_INCREMENT = 47
DEFAULT CHARACTER SET = latin1;


-- -----------------------------------------------------
-- Table `GAMIFICPRDNEW`.`Firm_Campaign`
-- -----------------------------------------------------
DROP TABLE IF EXISTS `GAMIFICPRDNEW`.`Firm_Campaign` ;

CREATE TABLE IF NOT EXISTS `GAMIFICPRDNEW`.`Firm_Campaign` (
  `Id` INT(11) NOT NULL AUTO_INCREMENT,
  `FirmId` INT(11) NOT NULL,
  `CreatedBy` INT(11) NOT NULL,
  `WorkerTypeId` INT(11) NOT NULL,
  `InitialDate` DATETIME NOT NULL,
  `EndDate` DATETIME NOT NULL,
  `CampaignName` VARCHAR(50) NOT NULL,
  `Description` VARCHAR(500) NOT NULL,
  `UpdatedBy` INT(11) NOT NULL,
  `Status` TINYINT(4) NOT NULL,
  `LastUpdate` DATETIME NOT NULL,
  PRIMARY KEY (`Id`),
  INDEX `FIRM_ID_CAMPAIGN` (`FirmId` ASC),
  INDEX `SPONSOR_ID_CAMPAIGN` (`CreatedBy` ASC),
  INDEX `WORKER_TYPE_ID_CAMPAIGN` (`WorkerTypeId` ASC),
  CONSTRAINT `FIRM_ID_CAMPAIGN`
    FOREIGN KEY (`FirmId`)
    REFERENCES `GAMIFICPRDNEW`.`Firm_Data` (`Id`)
    ON DELETE NO ACTION
    ON UPDATE NO ACTION,
  CONSTRAINT `SPONSOR_ID_CAMPAIGN`
    FOREIGN KEY (`CreatedBy`)
    REFERENCES `GAMIFICPRDNEW`.`Firm_Worker` (`Id`)
    ON DELETE NO ACTION
    ON UPDATE NO ACTION,
  CONSTRAINT `WORKER_TYPE_ID_CAMPAIGN`
    FOREIGN KEY (`WorkerTypeId`)
    REFERENCES `GAMIFICPRDNEW`.`Firm_Worker_Type` (`Id`)
    ON DELETE NO ACTION
    ON UPDATE NO ACTION)
ENGINE = InnoDB
DEFAULT CHARACTER SET = latin1;


-- -----------------------------------------------------
-- Table `GAMIFICPRDNEW`.`Firm_Goal`
-- -----------------------------------------------------
DROP TABLE IF EXISTS `GAMIFICPRDNEW`.`Firm_Goal` ;

CREATE TABLE IF NOT EXISTS `GAMIFICPRDNEW`.`Firm_Goal` (
  `Id` INT(11) NOT NULL AUTO_INCREMENT,
  `ExternalMetricId` VARCHAR(25) NOT NULL,
  `EpisodeId` VARCHAR(25) NOT NULL,
  `RunId` VARCHAR(25) NOT NULL,
  `Goal` INT(11) NOT NULL,
  `UpdatedBy` INT(11) NOT NULL,
  `LastUpdate` DATETIME NOT NULL,
  PRIMARY KEY (`Id`))
ENGINE = InnoDB
AUTO_INCREMENT = 98
DEFAULT CHARACTER SET = latin1;


-- -----------------------------------------------------
-- Table `GAMIFICPRDNEW`.`Firm_Team`
-- -----------------------------------------------------
DROP TABLE IF EXISTS `GAMIFICPRDNEW`.`Firm_Team` ;

CREATE TABLE IF NOT EXISTS `GAMIFICPRDNEW`.`Firm_Team` (
  `Id` INT(11) NOT NULL AUTO_INCREMENT,
  `TeamName` VARCHAR(50) NOT NULL,
  `FirmId` INT(11) NOT NULL,
  `SponsorId` INT(11) NOT NULL,
  `WorkerTypeId` INT(11) NOT NULL,
  `UpdatedBy` INT(11) NOT NULL,
  `Status` TINYINT(4) NOT NULL,
  `LogoId` INT(11) NULL DEFAULT '0',
  `LastUpdate` DATETIME NOT NULL,
  `ExternalId` VARCHAR(25) NULL DEFAULT NULL,
  PRIMARY KEY (`Id`),
  UNIQUE INDEX `IdExterno` (`ExternalId` ASC),
  INDEX `FIRM_ID_TEAM` (`FirmId` ASC),
  INDEX `SPONSOR_ID_TEAM` (`SponsorId` ASC),
  INDEX `WORKER_TYPE_ID_TEAM` (`WorkerTypeId` ASC),
  CONSTRAINT `FIRM_ID_TEAM`
    FOREIGN KEY (`FirmId`)
    REFERENCES `GAMIFICPRDNEW`.`Firm_Data` (`Id`)
    ON DELETE NO ACTION
    ON UPDATE NO ACTION,
  CONSTRAINT `SPONSOR_ID_TEAM`
    FOREIGN KEY (`SponsorId`)
    REFERENCES `GAMIFICPRDNEW`.`Firm_Worker` (`Id`)
    ON DELETE NO ACTION
    ON UPDATE NO ACTION,
  CONSTRAINT `WORKER_TYPE_ID_TEAM`
    FOREIGN KEY (`WorkerTypeId`)
    REFERENCES `GAMIFICPRDNEW`.`Firm_Worker_Type` (`Id`)
    ON DELETE NO ACTION
    ON UPDATE NO ACTION)
ENGINE = InnoDB
AUTO_INCREMENT = 10
DEFAULT CHARACTER SET = latin1;


-- -----------------------------------------------------
-- Table `GAMIFICPRDNEW`.`Firm_Message`
-- -----------------------------------------------------
DROP TABLE IF EXISTS `GAMIFICPRDNEW`.`Firm_Message` ;

CREATE TABLE IF NOT EXISTS `GAMIFICPRDNEW`.`Firm_Message` (
  `Id` INT(11) NOT NULL AUTO_INCREMENT,
  `FirmId` INT(11) NOT NULL,
  `Sender` INT(11) NOT NULL,
  `TeamId` INT(11) NOT NULL,
  `SendDateTime` DATETIME NOT NULL,
  `Message` TEXT NOT NULL,
  PRIMARY KEY (`Id`),
  INDEX `FIRM_ID_MESSAGE` (`FirmId` ASC),
  INDEX `SENDER_ID_MESSAGE` (`Sender` ASC),
  INDEX `TEAM_ID_MESSAGE` (`TeamId` ASC),
  CONSTRAINT `FIRM_ID_MESSAGE`
    FOREIGN KEY (`FirmId`)
    REFERENCES `GAMIFICPRDNEW`.`Firm_Data` (`Id`)
    ON DELETE NO ACTION
    ON UPDATE NO ACTION,
  CONSTRAINT `SENDER_ID_MESSAGE`
    FOREIGN KEY (`Sender`)
    REFERENCES `GAMIFICPRDNEW`.`Account_UserAccount` (`Id`)
    ON DELETE NO ACTION
    ON UPDATE NO ACTION,
  CONSTRAINT `TEAM_ID_MESSAGE`
    FOREIGN KEY (`TeamId`)
    REFERENCES `GAMIFICPRDNEW`.`Firm_Team` (`Id`)
    ON DELETE NO ACTION
    ON UPDATE NO ACTION)
ENGINE = InnoDB
AUTO_INCREMENT = 47
DEFAULT CHARACTER SET = latin1;


-- -----------------------------------------------------
-- Table `GAMIFICPRDNEW`.`Firm_Metric`
-- -----------------------------------------------------
DROP TABLE IF EXISTS `GAMIFICPRDNEW`.`Firm_Metric` ;

CREATE TABLE IF NOT EXISTS `GAMIFICPRDNEW`.`Firm_Metric` (
  `Id` INT(11) NOT NULL AUTO_INCREMENT,
  `MetricName` VARCHAR(100) NOT NULL,
  `FirmId` INT(11) NOT NULL,
  `UpdatedBy` INT(11) NOT NULL,
  `LastUpdate` DATETIME NOT NULL,
  `Status` TINYINT(4) NOT NULL,
  `Icon` VARCHAR(50) NOT NULL,
  `MinValue` INT(11) NULL DEFAULT NULL,
  `Weigth` INT(11) NOT NULL,
  `ValueMax` INT(11) NULL DEFAULT NULL,
  `ExternalID` VARCHAR(200) NULL DEFAULT NULL,
  PRIMARY KEY (`Id`),
  INDEX `FIRM_ID_METRIC` (`FirmId` ASC),
  CONSTRAINT `FIRM_ID_METRIC`
    FOREIGN KEY (`FirmId`)
    REFERENCES `GAMIFICPRDNEW`.`Firm_Data` (`Id`)
    ON DELETE NO ACTION
    ON UPDATE NO ACTION)
ENGINE = InnoDB
AUTO_INCREMENT = 17
DEFAULT CHARACTER SET = latin1;


-- -----------------------------------------------------
-- Table `GAMIFICPRDNEW`.`Firm_Param`
-- -----------------------------------------------------
DROP TABLE IF EXISTS `GAMIFICPRDNEW`.`Firm_Param` ;

CREATE TABLE IF NOT EXISTS `GAMIFICPRDNEW`.`Firm_Param` (
  `Id` INT(11) UNSIGNED NOT NULL AUTO_INCREMENT,
  `Name` VARCHAR(100) NOT NULL,
  `Value` VARCHAR(100) NOT NULL,
  `Description` VARCHAR(500) NOT NULL,
  `GameId` VARCHAR(25) NOT NULL,
  `LastUpdate` DATETIME NOT NULL,
  `UpdateBy` VARCHAR(100) NOT NULL,
  PRIMARY KEY (`Id`),
  UNIQUE INDEX `uniqueName` (`Name` ASC, `GameId` ASC),
  INDEX `external_id` (`GameId` ASC))
ENGINE = InnoDB
AUTO_INCREMENT = 19
DEFAULT CHARACTER SET = latin1;


-- -----------------------------------------------------
-- Table `GAMIFICPRDNEW`.`Firm_Player_Run`
-- -----------------------------------------------------
DROP TABLE IF EXISTS `GAMIFICPRDNEW`.`Firm_Player_Run` ;

CREATE TABLE IF NOT EXISTS `GAMIFICPRDNEW`.`Firm_Player_Run` (
  `Id` INT(11) NOT NULL AUTO_INCREMENT,
  `WorkerId` INT(11) NOT NULL,
  `ExternalTeamId` VARCHAR(100) NOT NULL,
  `EpisodeId` VARCHAR(100) NOT NULL,
  `RunId` VARCHAR(100) NOT NULL,
  `UpdatedBy` INT(11) NOT NULL,
  `Status` TINYINT(4) NOT NULL,
  PRIMARY KEY (`Id`),
  INDEX `WORKER_ID_PLAYER_RUN` (`WorkerId` ASC),
  CONSTRAINT `WORKER_ID_PLAYER_RUN`
    FOREIGN KEY (`WorkerId`)
    REFERENCES `GAMIFICPRDNEW`.`Firm_Worker` (`Id`)
    ON DELETE NO ACTION
    ON UPDATE NO ACTION)
ENGINE = InnoDB
AUTO_INCREMENT = 3
DEFAULT CHARACTER SET = latin1;


-- -----------------------------------------------------
-- Table `GAMIFICPRDNEW`.`Firm_Result`
-- -----------------------------------------------------
DROP TABLE IF EXISTS `GAMIFICPRDNEW`.`Firm_Result` ;

CREATE TABLE IF NOT EXISTS `GAMIFICPRDNEW`.`Firm_Result` (
  `Id` INT(11) NOT NULL AUTO_INCREMENT,
  `FirmId` INT(11) NOT NULL,
  `WorkerId` INT(11) NOT NULL,
  `MetricId` INT(11) NOT NULL,
  `Period` DATETIME NOT NULL,
  `UpdatedBy` INT(11) NOT NULL,
  `LastUpdate` DATETIME NOT NULL,
  `Result` INT(11) NOT NULL,
  `MainResult` INT(11) NULL DEFAULT NULL,
  PRIMARY KEY (`Id`),
  INDEX `FIRM_ID_RESULT` (`FirmId` ASC),
  INDEX `WORKER_ID_RESULT` (`WorkerId` ASC),
  INDEX `METRIC_ID_RESULT` (`MetricId` ASC),
  INDEX `MainResult` (`MainResult` ASC),
  CONSTRAINT `FIRM_ID_RESULT`
    FOREIGN KEY (`FirmId`)
    REFERENCES `GAMIFICPRDNEW`.`Firm_Data` (`Id`)
    ON DELETE NO ACTION
    ON UPDATE NO ACTION,
  CONSTRAINT `Firm_Result_ibfk_1`
    FOREIGN KEY (`MainResult`)
    REFERENCES `GAMIFICPRDNEW`.`Firm_Result` (`Id`),
  CONSTRAINT `METRIC_ID_RESULT`
    FOREIGN KEY (`MetricId`)
    REFERENCES `GAMIFICPRDNEW`.`Firm_Metric` (`Id`)
    ON DELETE NO ACTION
    ON UPDATE NO ACTION,
  CONSTRAINT `WORKER_ID_RESULT`
    FOREIGN KEY (`WorkerId`)
    REFERENCES `GAMIFICPRDNEW`.`Firm_Worker` (`Id`)
    ON DELETE NO ACTION
    ON UPDATE NO ACTION)
ENGINE = InnoDB
AUTO_INCREMENT = 7
DEFAULT CHARACTER SET = latin1;


-- -----------------------------------------------------
-- Table `GAMIFICPRDNEW`.`Firm_Team_Worker`
-- -----------------------------------------------------
DROP TABLE IF EXISTS `GAMIFICPRDNEW`.`Firm_Team_Worker` ;

CREATE TABLE IF NOT EXISTS `GAMIFICPRDNEW`.`Firm_Team_Worker` (
  `Id` INT(11) NOT NULL AUTO_INCREMENT,
  `ExternalTeamId` VARCHAR(25) NOT NULL,
  `ExternalWorkerId` VARCHAR(25) NOT NULL,
  `FirmId` INT(11) NOT NULL,
  `TeamId` INT(11) NULL DEFAULT NULL,
  `WorkerId` INT(11) NULL DEFAULT NULL,
  `UpdatedBy` INT(11) NOT NULL,
  `LastUpdate` DATETIME NOT NULL,
  `Status` TINYINT(4) NOT NULL,
  PRIMARY KEY (`Id`),
  INDEX `FIRM_ID_TEAM_WORKER` (`FirmId` ASC),
  CONSTRAINT `FIRM_ID_TEAM_WORKER`
    FOREIGN KEY (`FirmId`)
    REFERENCES `GAMIFICPRDNEW`.`Firm_Data` (`Id`)
    ON DELETE NO ACTION
    ON UPDATE NO ACTION)
ENGINE = InnoDB
AUTO_INCREMENT = 18
DEFAULT CHARACTER SET = latin1;


-- -----------------------------------------------------
-- Table `GAMIFICPRDNEW`.`Firm_Video`
-- -----------------------------------------------------
DROP TABLE IF EXISTS `GAMIFICPRDNEW`.`Firm_Video` ;

CREATE TABLE IF NOT EXISTS `GAMIFICPRDNEW`.`Firm_Video` (
  `Id` INT(11) NOT NULL AUTO_INCREMENT,
  `VideoUrl` TEXT NOT NULL,
  `FirmId` INT(11) NOT NULL,
  `VideoTitle` VARCHAR(100) NOT NULL,
  `Status` TINYINT(4) NOT NULL,
  `UpdatedBy` INT(11) NOT NULL,
  `LastUpdate` DATETIME NOT NULL,
  PRIMARY KEY (`Id`))
ENGINE = InnoDB
DEFAULT CHARACTER SET = latin1;


-- -----------------------------------------------------
-- Table `GAMIFICPRDNEW`.`Firm_Video_Question`
-- -----------------------------------------------------
DROP TABLE IF EXISTS `GAMIFICPRDNEW`.`Firm_Video_Question` ;

CREATE TABLE IF NOT EXISTS `GAMIFICPRDNEW`.`Firm_Video_Question` (
  `Id` INT(11) NOT NULL AUTO_INCREMENT,
  `QuestionName` VARCHAR(100) NOT NULL,
  `CorrectAnswer` TEXT NOT NULL,
  `Answers` TEXT NOT NULL,
  `VideoId` INT(11) NOT NULL,
  `Status` TINYINT(4) NOT NULL,
  `FirmId` INT(11) NOT NULL,
  `UpdatedBy` INT(11) NOT NULL,
  `LastUpdate` DATETIME NOT NULL,
  PRIMARY KEY (`Id`),
  INDEX `VIDEO_ID` (`VideoId` ASC),
  CONSTRAINT `VIDEO_ID`
    FOREIGN KEY (`VideoId`)
    REFERENCES `GAMIFICPRDNEW`.`Firm_Video` (`Id`)
    ON DELETE NO ACTION
    ON UPDATE NO ACTION)
ENGINE = InnoDB
DEFAULT CHARACTER SET = latin1;


-- -----------------------------------------------------
-- Table `GAMIFICPRDNEW`.`Firm_Video_Question_Answered`
-- -----------------------------------------------------
DROP TABLE IF EXISTS `GAMIFICPRDNEW`.`Firm_Video_Question_Answered` ;

CREATE TABLE IF NOT EXISTS `GAMIFICPRDNEW`.`Firm_Video_Question_Answered` (
  `Id` INT(11) NOT NULL AUTO_INCREMENT,
  `VideoQuestionId` INT(11) NOT NULL,
  `UserId` INT(11) NOT NULL,
  `Answer` TEXT NOT NULL,
  `AnsweredDate` DATETIME NOT NULL,
  PRIMARY KEY (`Id`),
  INDEX `VIDEO_QUESTION_ID` (`VideoQuestionId` ASC),
  INDEX `USER_ID_VIDEO_QUESTION_ANSWERED` (`UserId` ASC),
  CONSTRAINT `USER_ID_VIDEO_QUESTION_ANSWERED`
    FOREIGN KEY (`UserId`)
    REFERENCES `GAMIFICPRDNEW`.`Account_UserAccount` (`Id`)
    ON DELETE NO ACTION
    ON UPDATE NO ACTION,
  CONSTRAINT `VIDEO_QUESTION_ID`
    FOREIGN KEY (`VideoQuestionId`)
    REFERENCES `GAMIFICPRDNEW`.`Firm_Video_Question` (`Id`)
    ON DELETE NO ACTION
    ON UPDATE NO ACTION)
ENGINE = InnoDB
DEFAULT CHARACTER SET = latin1;


-- -----------------------------------------------------
-- Table `GAMIFICPRDNEW`.`Firm_Worker_Type_Metric`
-- -----------------------------------------------------
DROP TABLE IF EXISTS `GAMIFICPRDNEW`.`Firm_Worker_Type_Metric` ;

CREATE TABLE IF NOT EXISTS `GAMIFICPRDNEW`.`Firm_Worker_Type_Metric` (
  `Id` INT(11) NOT NULL AUTO_INCREMENT,
  `MetricId` INT(11) NULL DEFAULT NULL,
  `MetricExternalId` VARCHAR(25) NOT NULL,
  `WorkerTypeId` INT(11) NOT NULL,
  `UpdatedBy` INT(11) NOT NULL,
  `LastUpdate` DATETIME NOT NULL,
  `Status` TINYINT(4) NOT NULL,
  PRIMARY KEY (`Id`),
  INDEX `METRIC_ID_WORKER_TYPE_METRIC` (`MetricId` ASC),
  INDEX `WORKER_TYPE_ID_WORKER_TYPE_METRIC` (`WorkerTypeId` ASC),
  CONSTRAINT `METRIC_ID_WORKER_TYPE_METRIC`
    FOREIGN KEY (`MetricId`)
    REFERENCES `GAMIFICPRDNEW`.`Firm_Metric` (`Id`)
    ON DELETE NO ACTION
    ON UPDATE NO ACTION,
  CONSTRAINT `WORKER_TYPE_ID_WORKER_TYPE_METRIC`
    FOREIGN KEY (`WorkerTypeId`)
    REFERENCES `GAMIFICPRDNEW`.`Firm_Worker_Type` (`Id`)
    ON DELETE NO ACTION
    ON UPDATE NO ACTION)
ENGINE = InnoDB
AUTO_INCREMENT = 56
DEFAULT CHARACTER SET = latin1;


-- -----------------------------------------------------
-- Table `GAMIFICPRDNEW`.`Media_Image`
-- -----------------------------------------------------
DROP TABLE IF EXISTS `GAMIFICPRDNEW`.`Media_Image` ;

CREATE TABLE IF NOT EXISTS `GAMIFICPRDNEW`.`Media_Image` (
  `Id` INT(11) NOT NULL AUTO_INCREMENT,
  `UpdatedBy` INT(11) NOT NULL,
  `Status` TINYINT(4) NOT NULL,
  `LastUpdate` DATETIME NOT NULL,
  PRIMARY KEY (`Id`))
ENGINE = InnoDB
AUTO_INCREMENT = 57
DEFAULT CHARACTER SET = latin1;


-- -----------------------------------------------------
-- Table `GAMIFICPRDNEW`.`Public_TopicHelp`
-- -----------------------------------------------------
DROP TABLE IF EXISTS `GAMIFICPRDNEW`.`Public_TopicHelp` ;

CREATE TABLE IF NOT EXISTS `GAMIFICPRDNEW`.`Public_TopicHelp` (
  `Id` INT(11) NOT NULL AUTO_INCREMENT,
  `Status` TINYINT(4) NOT NULL,
  `TopicName` VARCHAR(100) NOT NULL,
  `FirmId` INT(11) NOT NULL,
  `LastUpdate` DATETIME NOT NULL,
  `UpdatedBy` INT(11) NOT NULL,
  PRIMARY KEY (`Id`),
  INDEX `FIRM_ID_TOPIC_HELP` (`FirmId` ASC),
  CONSTRAINT `FIRM_ID_TOPIC_HELP`
    FOREIGN KEY (`FirmId`)
    REFERENCES `GAMIFICPRDNEW`.`Firm_Data` (`Id`)
    ON DELETE NO ACTION
    ON UPDATE NO ACTION)
ENGINE = InnoDB
AUTO_INCREMENT = 5
DEFAULT CHARACTER SET = latin1;


-- -----------------------------------------------------
-- Table `GAMIFICPRDNEW`.`Public_Help`
-- -----------------------------------------------------
DROP TABLE IF EXISTS `GAMIFICPRDNEW`.`Public_Help` ;

CREATE TABLE IF NOT EXISTS `GAMIFICPRDNEW`.`Public_Help` (
  `Id` INT(11) NOT NULL AUTO_INCREMENT,
  `Status` TINYINT(4) NOT NULL,
  `FirmId` INT(11) NOT NULL,
  `TopicId` INT(11) NOT NULL,
  `LastUpdate` DATETIME NOT NULL,
  `HelpContent` VARCHAR(100) NOT NULL,
  `HelpTitle` TEXT NOT NULL,
  `UpdatedBy` INT(11) NOT NULL,
  PRIMARY KEY (`Id`),
  INDEX `FIRM_ID_HELP` (`FirmId` ASC),
  INDEX `TOPIC_ID_HELP` (`TopicId` ASC),
  CONSTRAINT `FIRM_ID_HELP`
    FOREIGN KEY (`FirmId`)
    REFERENCES `GAMIFICPRDNEW`.`Firm_Data` (`Id`)
    ON DELETE NO ACTION
    ON UPDATE NO ACTION,
  CONSTRAINT `TOPIC_ID_HELP`
    FOREIGN KEY (`TopicId`)
    REFERENCES `GAMIFICPRDNEW`.`Public_TopicHelp` (`Id`)
    ON DELETE NO ACTION
    ON UPDATE NO ACTION)
ENGINE = InnoDB
DEFAULT CHARACTER SET = latin1;


SET SQL_MODE=@OLD_SQL_MODE;
SET FOREIGN_KEY_CHECKS=@OLD_FOREIGN_KEY_CHECKS;
SET UNIQUE_CHECKS=@OLD_UNIQUE_CHECKS;
