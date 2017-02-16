CREATE TABLE `Account_UserAccount` (
  `Id` int(11) NOT NULL AUTO_INCREMENT,
  `PasswordHash` longtext,
  `SecurityStamp` longtext,
  `LockoutDate` datetime DEFAULT NULL,
  `AccessFailedCount` int(11) NOT NULL,
  `UserName` varchar(256) NOT NULL,
  `LastUpdate` datetime NOT NULL,
  `Status` tinyint(4) NOT NULL,
  PRIMARY KEY (`Id`),
  KEY `UserName_index` (`UserName`)
);

CREATE TABLE `Account_UserProfile` (
  `Id` int(11) NOT NULL,
  `Name` varchar(100) NOT NULL,
  `Email` varchar(100) NOT NULL,
  `CPF` varchar(20) DEFAULT NULL,
  `Phone` varchar(20) NOT NULL,
  `LastUpdate` datetime NOT NULL,
  PRIMARY KEY (`Id`)
);

CREATE TABLE `Account_UserRole` (
  `UserId` int(11) NOT NULL,
  `Role` int(11) NOT NULL,
  PRIMARY KEY (`UserId`,`Role`),
  KEY `Role_FK_idx` (`Role`),
  CONSTRAINT `UserRole_Account_FK` FOREIGN KEY (`UserId`) REFERENCES `Account_UserAccount` (`Id`) ON DELETE NO ACTION ON UPDATE NO ACTION
);

CREATE TABLE `Media_Image` (
  `Id` int(11) NOT NULL AUTO_INCREMENT,
  `UpdatedBy` int(11) NOT NULL,
  `Status` tinyint(4) NOT NULL,
  `LastUpdate` datetime NOT NULL,
  PRIMARY KEY (`Id`)
);

CREATE TABLE `Firm_Data` (
  `Id` int(11) NOT NULL AUTO_INCREMENT,
  `FirmName` varchar(100) NOT NULL,
  `CompanyName` varchar(100) NOT NULL,
  `CNPJ` varchar(100) NOT NULL,
  `LogoId` int(11) DEFAULT 0,
  `Adress` varchar(100) NOT NULL,
  `Neighborhood` varchar(100) NOT NULL,
  `City` varchar(100) NOT NULL,
  `UpdatedBy` int(11) NOT NULL,
  `Status` tinyint(4) NOT NULL,
  `LastUpdate` datetime NOT NULL,
  `Phone` varchar(30) NOT NULL,
  PRIMARY KEY (`Id`)
);

CREATE TABLE `Firm_Worker_Type` (
  `Id` int(11) NOT NULL AUTO_INCREMENT,
  `TypeName` varchar(50) NOT NULL,
  `ProfileName` int(11) NOT NULL,
  `FirmId` int(11) NOT NULL,
  `UpdatedBy` int(11) NOT NULL,
  `Status` tinyint(4) NOT NULL,
  `LastUpdate` datetime NOT NULL,
  PRIMARY KEY (`Id`),
  KEY `FIRM_ID_PROFILE` (`FirmId`),
  CONSTRAINT `FIRM_ID_PROFILE` FOREIGN KEY (`FirmId`) REFERENCES `Firm_Data` (`Id`) ON DELETE NO ACTION ON UPDATE NO ACTION
);

CREATE TABLE `Firm_Worker` (
  `Id` int(11) NOT NULL AUTO_INCREMENT,
  `WorkerTypeId` int(11) NOT NULL,
  `UserId` int(11) NOT NULL,
  `IdExterno` int(11) UNIQUE,
  `FirmId` int(11) NOT NULL,
  `UpdatedBy` int(11) NOT NULL,
  `LogoId` int(11) DEFAULT 0,
  `Status` tinyint(4) NOT NULL,
  `LastUpdate` datetime NOT NULL,
  PRIMARY KEY (`Id`),
  KEY `FIRM_ID_WORKER` (`FirmId`),
  KEY `USER_ID_WORKER` (`UserId`),
  KEY `WORKER_TYPE_ID_WORKER` (`WorkerTypeId`),
  CONSTRAINT `FIRM_ID_WORKER` FOREIGN KEY (`FirmId`) REFERENCES `Firm_Data` (`Id`) ON DELETE NO ACTION ON UPDATE NO ACTION,
  CONSTRAINT `WORKER_TYPE_ID_WORKER` FOREIGN KEY (`WorkerTypeId`) REFERENCES `Firm_Worker_Type` (`Id`) ON DELETE NO ACTION ON UPDATE NO ACTION,
  CONSTRAINT `USER_ID_WORKER` FOREIGN KEY (`UserId`) REFERENCES `Account_UserAccount` (`Id`) ON DELETE NO ACTION ON UPDATE NO ACTION
);

CREATE TABLE `Public_TopicHelp` (
  `Id` int(11) NOT NULL AUTO_INCREMENT,
  `Status` tinyint(4) NOT NULL,
  `TopicName` varchar(100) NOT NULL,
  `FirmId` int(11) NOT NULL,
  `LastUpdate` datetime NOT NULL,
  `UpdatedBy` int(11) NOT NULL,
  PRIMARY KEY (`Id`),
  KEY `FIRM_ID_TOPIC_HELP` (`FirmId`),
  CONSTRAINT `FIRM_ID_TOPIC_HELP` FOREIGN KEY (`FirmId`) REFERENCES `Firm_Data` (`Id`) ON DELETE NO ACTION ON UPDATE NO ACTION
);

CREATE TABLE `Public_Help` (
  `Id` int(11) NOT NULL AUTO_INCREMENT,
  `Status` tinyint(4) NOT NULL,
  `FirmId` int(11) NOT NULL,
  `TopicId` int(11) NOT NULL,
  `LastUpdate` datetime NOT NULL,
  `HelpContent` varchar(100) NOT NULL,
  `HelpTitle` text NOT NULL,
  `UpdatedBy` int(11) NOT NULL,
  PRIMARY KEY (`Id`),
  KEY `FIRM_ID_HELP` (`FirmId`),
  KEY `TOPIC_ID_HELP` (`TopicId`),
  CONSTRAINT `FIRM_ID_HELP` FOREIGN KEY (`FirmId`) REFERENCES `Firm_Data` (`Id`) ON DELETE NO ACTION ON UPDATE NO ACTION,
  CONSTRAINT `TOPIC_ID_HELP` FOREIGN KEY (`TopicId`) REFERENCES `Public_TopicHelp` (`Id`) ON DELETE NO ACTION ON UPDATE NO ACTION
);

CREATE TABLE `Firm_Team` (
  `Id` int(11) NOT NULL AUTO_INCREMENT,
  `TeamName` varchar(50) NOT NULL,
  `FirmId` int(11) NOT NULL,
  `IdExterno` int(11) UNIQUE,
  `SponsorId` int(11) NOT NULL,
  `WorkerTypeId` int(11) NOT NULL,
  `UpdatedBy` int(11) NOT NULL,
  `Status` tinyint(4) NOT NULL,
  `LogoId` int(11) DEFAULT 0,
  `LastUpdate` datetime NOT NULL,
  PRIMARY KEY (`Id`),
  KEY `FIRM_ID_TEAM` (`FirmId`),
  KEY `SPONSOR_ID_TEAM` (`SponsorId`),
  KEY `WORKER_TYPE_ID_TEAM` (`WorkerTypeId`),
  CONSTRAINT `FIRM_ID_TEAM` FOREIGN KEY (`FirmId`) REFERENCES `Firm_Data` (`Id`) ON DELETE NO ACTION ON UPDATE NO ACTION,
  CONSTRAINT `WORKER_TYPE_ID_TEAM` FOREIGN KEY (`WorkerTypeId`) REFERENCES `Firm_Worker_Type` (`Id`) ON DELETE NO ACTION ON UPDATE NO ACTION,
  CONSTRAINT `SPONSOR_ID_TEAM` FOREIGN KEY (`SponsorId`) REFERENCES `Firm_Worker` (`Id`) ON DELETE NO ACTION ON UPDATE NO ACTION
);

CREATE TABLE `Firm_Campaign` (
  `Id` int(11) NOT NULL AUTO_INCREMENT,
  `FirmId` int(11) NOT NULL,
  `CreatedBy` int(11) NOT NULL,
  `WorkerTypeId` int(11) NOT NULL,
  `InitialDate` datetime NOT NULL,
  `EndDate` datetime NOT NULL,
  `CampaignName` varchar(50) NOT NULL,
  `Description` varchar(500) NOT NULL,
  `UpdatedBy` int(11) NOT NULL,
  `Status` tinyint(4) NOT NULL,
  `LastUpdate` datetime NOT NULL,
  PRIMARY KEY (`Id`),
  KEY `FIRM_ID_CAMPAIGN` (`FirmId`),
  KEY `SPONSOR_ID_CAMPAIGN` (`CreatedBy`),
  KEY `WORKER_TYPE_ID_CAMPAIGN` (`WorkerTypeId`),
  CONSTRAINT `FIRM_ID_CAMPAIGN` FOREIGN KEY (`FirmId`) REFERENCES `Firm_Data` (`Id`) ON DELETE NO ACTION ON UPDATE NO ACTION,
  CONSTRAINT `WORKER_TYPE_ID_CAMPAIGN` FOREIGN KEY (`WorkerTypeId`) REFERENCES `Firm_Worker_Type` (`Id`) ON DELETE NO ACTION ON UPDATE NO ACTION,
  CONSTRAINT `SPONSOR_ID_CAMPAIGN` FOREIGN KEY (`CreatedBy`) REFERENCES `Firm_Worker` (`Id`) ON DELETE NO ACTION ON UPDATE NO ACTION
);

CREATE TABLE `Firm_Metric` (
  `Id` int(11) NOT NULL AUTO_INCREMENT,
  `MetricName` varchar(100) NOT NULL,
  `FirmId` int(11) NOT NULL,
  `UpdatedBy` int(11) NOT NULL,
  `LastUpdate` datetime NOT NULL,
  `Status` tinyint(4) NOT NULL,
  `Icon` varchar(50) NOT NULL,
  `MinValue` int(11) DEFAULT NULL,
  `Weigth` int(11) NOT NULL,
  `ValueMax` int(11) DEFAULT NULL,
  PRIMARY KEY (`Id`),
  KEY `FIRM_ID_METRIC` (`FirmId`),
  CONSTRAINT `FIRM_ID_METRIC` FOREIGN KEY (`FirmId`) REFERENCES `Firm_Data` (`Id`) ON DELETE NO ACTION ON UPDATE NO ACTION
);

CREATE TABLE `Firm_Team_Worker` (
  `Id` int(11) NOT NULL AUTO_INCREMENT,
  `FirmId` int(11) NOT NULL,
  `TeamId` int(11) NOT NULL,
  `WorkerId` int(11) NOT NULL,
  `UpdatedBy` int(11) NOT NULL,
  `LastUpdate` datetime NOT NULL,
  `Status` tinyint(4) NOT NULL,
  PRIMARY KEY (`Id`),
  KEY `FIRM_ID_TEAM_WORKER` (`FirmId`),
  KEY `TEAM_ID_TEAM_WORKER` (`TeamId`),
  KEY `WORKER_ID_TEAM_WORKER` (`WorkerId`),
  CONSTRAINT `FIRM_ID_TEAM_WORKER` FOREIGN KEY (`FirmId`) REFERENCES `Firm_Data` (`Id`) ON DELETE NO ACTION ON UPDATE NO ACTION,
  CONSTRAINT `TEAM_ID_TEAM_WORKER` FOREIGN KEY (`TeamId`) REFERENCES `Firm_Team` (`Id`) ON DELETE NO ACTION ON UPDATE NO ACTION,
  CONSTRAINT `WORKER_ID_TEAM_WORKER` FOREIGN KEY (`WorkerId`) REFERENCES `Firm_Worker` (`Id`) ON DELETE NO ACTION ON UPDATE NO ACTION
);

CREATE TABLE `Firm_Result` (
  `Id` int(11) NOT NULL AUTO_INCREMENT,
  `FirmId` int(11) NOT NULL,
  `WorkerId` int(11) NOT NULL,
  `MetricId` int(11) NOT NULL,
  `Period` datetime NOT NULL,
  `UpdatedBy` int(11) NOT NULL,
  `LastUpdate` datetime NOT NULL,
  `Result` int(11) NOT NULL,
  PRIMARY KEY (`Id`),
  KEY `FIRM_ID_RESULT` (`FirmId`),
  KEY `WORKER_ID_RESULT` (`WorkerId`),
  KEY `METRIC_ID_RESULT` (`MetricId`),
  CONSTRAINT `METRIC_ID_RESULT` FOREIGN KEY (`MetricId`) REFERENCES `Firm_Metric` (`Id`) ON DELETE NO ACTION ON UPDATE NO ACTION,
  CONSTRAINT `FIRM_ID_RESULT` FOREIGN KEY (`FirmId`) REFERENCES `Firm_Data` (`Id`) ON DELETE NO ACTION ON UPDATE NO ACTION,
  CONSTRAINT `WORKER_ID_RESULT` FOREIGN KEY (`WorkerId`) REFERENCES `Firm_Worker` (`Id`) ON DELETE NO ACTION ON UPDATE NO ACTION
);

CREATE TABLE `Firm_Message` (
  `Id` int(11) NOT NULL AUTO_INCREMENT,
  `FirmId` int(11) NOT NULL,
  `Sender` int(11) NOT NULL,
  `TeamId` int(11) NOT NULL,
  `SendDateTime` datetime NOT NULL,
  `Message` text NOT NULL,
  PRIMARY KEY (`Id`),
  KEY `FIRM_ID_MESSAGE` (`FirmId`),
  KEY `SENDER_ID_MESSAGE` (`Sender`),
  KEY `TEAM_ID_MESSAGE` (`TeamId`),
  CONSTRAINT `FIRM_ID_MESSAGE` FOREIGN KEY (`FirmId`) REFERENCES `Firm_Data` (`Id`) ON DELETE NO ACTION ON UPDATE NO ACTION,
  CONSTRAINT `SENDER_ID_MESSAGE` FOREIGN KEY (`Sender`) REFERENCES `Account_UserAccount` (`Id`) ON DELETE NO ACTION ON UPDATE NO ACTION,
  CONSTRAINT `TEAM_ID_MESSAGE` FOREIGN KEY (`TeamId`) REFERENCES `Firm_Team` (`Id`) ON DELETE NO ACTION ON UPDATE NO ACTION
);

CREATE TABLE `Firm_Video` (
  `Id` int(11) NOT NULL AUTO_INCREMENT,
  `VideoUrl` text NOT NULL,
  `FirmId` int(11) NOT NULL,
  `VideoTitle` varchar(100) NOT NULL,
  `Status` tinyint(4) NOT NULL,
  `UpdatedBy` int(11) NOT NULL,
  `LastUpdate` datetime NOT NULL,
   PRIMARY KEY (`Id`)
);

CREATE TABLE `Firm_Video_Question` (
  `Id` int(11) NOT NULL AUTO_INCREMENT,
  `QuestionName` varchar(100) NOT NULL,
  `CorrectAnswer` text NOT NULL,
  `Answers` text NOT NULL,
  `VideoId` int(11) NOT NULL,
  `Status` tinyint(4) NOT NULL,
  `FirmId` int(11) NOT NULL,
  `UpdatedBy` int(11) NOT NULL,
  `LastUpdate` datetime NOT NULL,
  PRIMARY KEY (`Id`),
  KEY `VIDEO_ID` (`VideoId`),
  CONSTRAINT `VIDEO_ID` FOREIGN KEY (`VideoId`) REFERENCES `Firm_Video` (`Id`) ON DELETE NO ACTION ON UPDATE NO ACTION
);

CREATE TABLE `Firm_Video_Question_Answered` (
  `Id` int(11) NOT NULL AUTO_INCREMENT,
  `VideoQuestionId` int(11) NOT NULL,
  `UserId` int(11) NOT NULL,
  `Answer` text NOT NULL,
  `AnsweredDate` datetime NOT NULL,
   PRIMARY KEY (`Id`),
   KEY `VIDEO_QUESTION_ID` (`VideoQuestionId`),
   CONSTRAINT `VIDEO_QUESTION_ID` FOREIGN KEY (`VideoQuestionId`) REFERENCES `Firm_Video_Question` (`Id`) ON DELETE NO ACTION ON UPDATE NO ACTION,
   KEY `USER_ID_VIDEO_QUESTION_ANSWERED` (`UserId`),
   CONSTRAINT `USER_ID_VIDEO_QUESTION_ANSWERED` FOREIGN KEY (`UserId`) REFERENCES `Account_UserAccount` (`Id`) ON DELETE NO ACTION ON UPDATE NO ACTION
);

CREATE TABLE `Firm_Worker_Type_Metric` (
  `Id` int(11) NOT NULL AUTO_INCREMENT,
  `MetricId` int(11) NOT NULL,
  `WorkerTypeId` int(11) NOT NULL,
  `UpdatedBy` int(11) NOT NULL,
  `LastUpdate` datetime NOT NULL,
  `Status` tinyint(4) NOT NULL,
  PRIMARY KEY (`Id`),
  KEY `METRIC_ID_WORKER_TYPE_METRIC` (`MetricId`),
  KEY `WORKER_TYPE_ID_WORKER_TYPE_METRIC` (`WorkerTypeId`),
  CONSTRAINT `METRIC_ID_WORKER_TYPE_METRIC` FOREIGN KEY (`MetricId`) REFERENCES `Firm_Metric` (`Id`) ON DELETE NO ACTION ON UPDATE NO ACTION,
  CONSTRAINT `WORKER_TYPE_ID_WORKER_TYPE_METRIC` FOREIGN KEY (`WorkerTypeId`) REFERENCES `Firm_Worker_Type` (`Id`) ON DELETE NO ACTION ON UPDATE NO ACTION
);

CREATE TABLE `Firm_Goal` (
  `Id` int(11) NOT NULL AUTO_INCREMENT,
  `MetricId` int(11) NOT NULL,
  `WorkerId` int(11) NOT NULL,
  `Goal` int (11) NOT NULL,
  `UpdatedBy` int(11) NOT NULL,
  `LastUpdate` datetime NOT NULL,
  PRIMARY KEY (`Id`),
  KEY `METRIC_ID_GOAL` (`MetricId`),
  KEY `WORKER_ID_GOAL` (`WorkerId`),
  CONSTRAINT `METRIC_ID_GOAL` FOREIGN KEY (`MetricId`) REFERENCES `Firm_Metric` (`Id`) ON DELETE NO ACTION ON UPDATE NO ACTION,
  CONSTRAINT `WORKER_ID_GOAL` FOREIGN KEY (`WorkerId`) REFERENCES `Firm_Worker` (`Id`) ON DELETE NO ACTION ON UPDATE NO ACTION
);

CREATE TABLE `Firm_Episode` (
  `Id` int(11) NOT NULL AUTO_INCREMENT,
  `Id_Externo` int(11) NOT NULL UNIQUE,
  `Name` int(11) NOT NULL,
  `InitialDate` datetime NOT NULL,
  `FinalDate` datetime NOT NULL,
  `FirmId` int(11) NOT NULL,
  `UpdatedBy` int(11) NOT NULL,
  `LastUpdate` datetime NOT NULL,
  `Status` tinyint(4) NOT NULL,
  PRIMARY KEY (`Id`),
  KEY `FIRM_ID_EPISODE` (`FirmId`),
  CONSTRAINT `FIRM_ID_EPISODE` FOREIGN KEY (`FirmId`) REFERENCES `Firm_Data` (`Id`) ON DELETE NO ACTION ON UPDATE NO ACTION
);

CREATE TABLE `Firm_Player_Run` (
  `Id` int(11) NOT NULL AUTO_INCREMENT,
  `WorkerId` int(11) NOT NULL,
  `TeamId` int(11) NOT NULL,
  `EpisodeId` int(11) NOT NULL,
  `RunId` int(11) NOT NULL,
  `UpdatedBy` int(11) NOT NULL,
  `Status` tinyint(4) NOT NULL,
  PRIMARY KEY (`Id`),
  KEY `WORKER_ID_PLAYER_RUN` (`WorkerId`),
  KEY `TEAM_ID_PLAYER_RUN` (`TeamId`),
  KEY `EPISODE_ID_PLAYER_RUN` (`EpisodeId`),
  CONSTRAINT `WORKER_ID_PLAYER_RUN` FOREIGN KEY (`WorkerId`) REFERENCES `Firm_Worker` (`Id`) ON DELETE NO ACTION ON UPDATE NO ACTION,
  CONSTRAINT `TEAM_ID_PLAYER_RUN` FOREIGN KEY (`TeamId`) REFERENCES `Firm_Team` (`Id`) ON DELETE NO ACTION ON UPDATE NO ACTION,
  CONSTRAINT `EPISODE_ID_PLAYER_RUN` FOREIGN KEY (`EpisodeId`) REFERENCES `Firm_Episode` (`Id`) ON DELETE NO ACTION ON UPDATE NO ACTION
);