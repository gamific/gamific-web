-- MySQL dump 10.13  Distrib 5.7.12, for osx10.9 (x86_64)
--
-- Host: gamificprd.cnrf5w7kyplg.us-east-1.rds.amazonaws.com    Database: GAMIFICPRDNEW
-- ------------------------------------------------------
-- Server version	5.6.27-log

/*!40101 SET @OLD_CHARACTER_SET_CLIENT=@@CHARACTER_SET_CLIENT */;
/*!40101 SET @OLD_CHARACTER_SET_RESULTS=@@CHARACTER_SET_RESULTS */;
/*!40101 SET @OLD_COLLATION_CONNECTION=@@COLLATION_CONNECTION */;
/*!40101 SET NAMES utf8 */;
/*!40103 SET @OLD_TIME_ZONE=@@TIME_ZONE */;
/*!40103 SET TIME_ZONE='+00:00' */;
/*!40014 SET @OLD_UNIQUE_CHECKS=@@UNIQUE_CHECKS, UNIQUE_CHECKS=0 */;
/*!40014 SET @OLD_FOREIGN_KEY_CHECKS=@@FOREIGN_KEY_CHECKS, FOREIGN_KEY_CHECKS=0 */;
/*!40101 SET @OLD_SQL_MODE=@@SQL_MODE, SQL_MODE='NO_AUTO_VALUE_ON_ZERO' */;
/*!40111 SET @OLD_SQL_NOTES=@@SQL_NOTES, SQL_NOTES=0 */;

--
-- Table structure for table `Account_UserAccount`
--

DROP TABLE IF EXISTS `Account_UserAccount`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!40101 SET character_set_client = utf8 */;
CREATE TABLE `Account_UserAccount` (
  `Id` int(11) NOT NULL AUTO_INCREMENT,
  `PasswordHash` longtext,
  `SecurityStamp` longtext,
  `LockoutDate` datetime DEFAULT NULL,
  `AccessFailedCount` int(11) NOT NULL,
  `UserName` varchar(256) NOT NULL,
  `LastUpdate` datetime NOT NULL,
  `Status` tinyint(4) NOT NULL,
  `lastLogin` datetime DEFAULT NULL,
  `tokenMobile` varchar(1000) DEFAULT NULL,
  `device` varchar(100) DEFAULT NULL,
  PRIMARY KEY (`Id`),
  KEY `UserName_index` (`UserName`)
) ENGINE=InnoDB AUTO_INCREMENT=52 DEFAULT CHARSET=latin1;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `Account_UserAccount`
--

LOCK TABLES `Account_UserAccount` WRITE;
/*!40000 ALTER TABLE `Account_UserAccount` DISABLE KEYS */;
INSERT INTO `Account_UserAccount` VALUES (1,'Ke/J6co3CFwq0ZSWU17QydfmbFaMJj+4','9SehgW7m9oZjesJ9VUmXMDm/+xpXX+Hz',NULL,0,'marcosvap','2017-01-11 11:43:26',1,'2017-01-11 11:43:26',NULL,NULL),(2,'TEtAUjGFTlDEqc6IvSNaFiKBCxd6yZjR','4pFePIg6Uyak7EE1WxT0tDe97HyBwQ0k',NULL,0,'paulo','2016-12-05 12:28:46',1,'2017-01-03 15:01:36',NULL,NULL),(3,'8TUXgucuxq4bXKHnE5MIAx2c0LkgiATS','IzENfRliL4OJmrh6A1MRaAop37qXOCnr',NULL,0,'adm','2017-01-17 11:37:04',1,'2017-01-17 11:37:04','cGZgue1VwJk:APA91bGTxiaFb8g8Xy0KX7IA80yEbmcJuzG5_bGEzQfmvedoi6iC3ywqDvEikbdy6QBUmzpAlr_0cPdlpfj7kFPR3nzwVAvaxbYDAZxf4X-2glLA8olSNb-JsEglvIkBzfUI3ciCbkHB','ANDROID'),(4,'8lfIsJ9U90bQ0Z2yK6tbOL9Psoc44Raj','T0PykpzzdCtwgcTqx3Rlj3rcSp78LOGc',NULL,0,'vendedor1@email.com.br','2016-12-05 13:38:07',1,'2017-01-03 15:01:36',NULL,NULL),(5,'dQ19RkTxRjoT+swxoqxGi/gq3v59WSNi','1jYmZaYFfqx4hs5o6nT+wPPVrKkhcqlu',NULL,0,'supervisor1@email.com.br','2016-12-05 13:38:39',1,'2017-01-03 15:01:36',NULL,NULL),(6,'s84WXITIZxonGAIAIX9CBsq364Nc7Rky','JgWGmARietYRk/PajzjtDMBtUJX8Dbfj',NULL,0,'vendedor2@email.com.br','2016-12-05 17:33:39',1,'2017-01-03 15:01:36',NULL,NULL),(7,'6kvvEYK69ANP8pOaXzUQX1LfzBCa3xIq','df+BZtuhbuudrWMfVbHMeciW1oXdcCJl',NULL,0,'gerente1@email.com.br','2016-12-05 17:34:07',1,'2017-01-03 15:01:36',NULL,NULL),(8,'L8WLJwLK6wDtmC14a7feVF9sADVPzXSf','RLBndcZT1JqhKPo1GJiTDwZ4AwM76Jwh',NULL,0,'priscilla@duplov.com.br','2016-12-06 10:49:51',1,'2017-01-03 15:01:36',NULL,NULL),(9,'stlmP0hbsOj+Kdd5vickXpcvfQcbZg4z','+doRnBcY/rNxr5yV/lJE+aqwJx6NF0gS',NULL,0,'guilherme@duplov.com.br','2016-12-06 10:50:00',1,'2017-01-03 15:01:36',NULL,NULL),(10,'6uSaqYr4jT5UudaZp5cqwQjjIT0ZVEHW','QiiCWw/0qOfCs6gnmu0iWveWuH6/CTH7',NULL,0,'hugo@duplov.com.br','2016-12-06 10:50:10',1,'2017-01-03 15:01:36',NULL,NULL),(11,'8U6Pm1MUAMYnADFKoimAobVDAVQwwaV6','odyqdwyyhwBErGoFb9At/z4Oo6mF10mS',NULL,0,'hugoadm@duplov.com.br','2016-12-06 10:50:25',1,'2017-01-03 15:01:36',NULL,NULL),(12,'31ZeqcZ0H1eYVpc4YCrWwLcd1He4otI2','tINOz/mxeOcwm/1ynQfWHb91gJBxPWl4',NULL,0,'glaucielly@duplov.com.br','2016-12-06 11:23:14',1,'2017-01-03 15:01:36',NULL,NULL),(13,'0J1q3IqeC/NuN0BYWKlQnzXbOpXK9VR6','t8egNP+nA7wFJygOLKcTd3gdlmYnmA/5',NULL,0,'alexsander@duplov.com.br','2016-12-06 11:24:32',1,'2017-01-03 15:01:36',NULL,NULL),(14,'+5JnKm/TFbJm1+b3Abb878/CIxMrCO3G','I/HC6JIwdrvn3P8vjdJ3sMllNut+RfzU',NULL,0,'caio@duplov.com.br','2016-12-06 11:25:17',1,'2017-01-03 15:01:36',NULL,NULL),(15,'YP9xE4Nwl87GUaftgFLwI9+y+lJjtiBn','YbMg9eUAm++KaHCvp1aioa6mrNAfNyVN',NULL,0,'gustavo@duplov.com.br','2016-12-06 11:25:58',1,'2017-01-03 15:01:36',NULL,NULL),(16,'ihawbsQIrBhXIWHKndnIbcztz6FJL3Yu','3jZk8Bbg4iVp7jtDdBPawQKvtGrxVa2i',NULL,0,'gustavoadm@duplov.com.br','2016-12-06 11:26:41',1,'2017-01-03 15:01:36',NULL,NULL),(17,'v0JZj4vfgbqnkdOwZaQzpevaROsAJVcH','QLi2PZgKCoYVHiQFdchdULI43f7f5Rb9',NULL,0,'rubens@duplov.com.br','2016-12-06 13:27:05',1,'2017-01-03 15:01:36',NULL,NULL),(18,'iXjsWVe/428YlrDKP3PQ5sMCruYhKBMc','BTh5Ps4kxztB1Jv5SMhSS1jiIe1uORBu',NULL,0,'flavio@duplov.com.br','2016-12-06 16:36:33',1,'2017-01-03 15:01:36',NULL,NULL),(20,'ZYvp8mVDDE9H4lhOJuwmnDrw4iomQarZ','xgkznSos2FLQJx66WLCzQC/cuf+r1Zvu',NULL,0,'igorgarantes@gmail.com','2016-12-19 16:16:50',1,'2017-01-03 15:01:36',NULL,NULL),(21,'RZMCaPhcvWsRb1OtvSej2Sal+gXxrphv','oDmXU5ohbQWgipfr0Cr55XoaBnG7BB0n',NULL,0,'rubs@email.com','2016-12-19 13:38:30',1,'2017-01-03 15:01:36',NULL,NULL),(22,'D4E9uGTE6QAS6YJvO5us2XxM2r0yhUBs','JlER66iiVunrt4HpTOzzSITTtXZXmt+m',NULL,0,'miller@gamific.com.br','2016-12-19 16:17:12',1,'2017-01-03 15:01:36',NULL,NULL),(23,'1+AIc+Z7glOJgQN0NZ4ERLvWfZqwHb0c','6VUJxmQrLA4roQI+i597iiKgIdqFw6Kj',NULL,0,'engine@email.com','2016-12-22 16:45:00',1,'2017-01-03 15:01:36',NULL,NULL),(24,'t03uSx0VGzW+Mz/OwWQYI+2z+ek4tnIe','o2wqaXY6SSAUMdZA5SnxGJjkn1WWJBfZ',NULL,0,'miller2@email.com','2016-12-22 18:56:06',0,'2017-01-03 15:01:36',NULL,NULL),(25,'JtRwpbJNAxaUtXEy7TMRYof388zXgXdG','illlYnBHRTGF2+uPcLe2Thz9GYk5+kzo',NULL,0,'miller3@email.com','2016-12-22 17:37:37',1,'2017-01-03 15:01:36',NULL,NULL),(26,'6Sj5XQyEAJ0R9l5vTcvXfHVAh1PDytSs','CJwm7vg4od8b3aanacBS9g3Aa2PHPJi4',NULL,0,'e','2016-12-22 19:03:08',0,'2017-01-03 15:01:36',NULL,NULL),(27,'drVmm0TT+dsPtgeo7TW1WG3lEN4vb3/b','jKYH5CWL/3oRFmLPJgGvsdKED+9Ljy+F',NULL,0,'isaodsa@dasd.com.br','2016-12-26 20:07:58',1,'2017-01-03 15:01:36',NULL,NULL),(28,'01m+2y6M1mx9GblonEDpHD51/ekJEKox','Rzszjo2e8JdG4kYH8nDFTSi29yQczgHj',NULL,0,'igor23@email.com','2016-12-27 17:12:29',1,'2017-01-03 15:01:36',NULL,NULL),(29,'BQkeo+F3WhjRbwLXuj/pUQEeZcKVhpy3','+DJSG7TP2L1wGlylYEx4EmA2GHSaLC+X',NULL,0,'igor12@email.com','2016-12-27 17:22:56',1,'2017-01-03 15:01:36',NULL,NULL),(30,'BZjuCSzJ9lUgV/K3pwtIPAhZTut3uhS2','GwAKyJmySA8VgVN7ALh0PAHICNeXHh4X',NULL,0,'jogador_11@email.com','2017-01-03 11:23:47',1,'2017-01-03 15:01:36',NULL,NULL),(31,'HnzJjv6zRbikPz1PItV5nqin3Kb0h3cw','m0oS2IXIrMQ87f2I6Q3F3VORcq0jYYX0',NULL,0,'jogador_12@email.com','2017-01-03 11:24:02',1,'2017-01-03 15:01:36',NULL,NULL),(32,'u139cjfgbZ3ofEhy7636LgU06mHhXhV8','oJKUqYj+TSIOaMNVxia5YAq75t2d4G/Y',NULL,0,'jogador_13@email.com','2017-01-03 11:24:32',1,'2017-01-03 15:01:36',NULL,NULL),(33,'vnWNXOjxUO8Kx/lLskC8BxgCTcs3nLiq','Uw1vyalVWHx2vaXtHz//0IWvGWIloqdl',NULL,0,'jogador_14@email.com','2017-01-03 11:24:52',1,'2017-01-03 15:01:36',NULL,NULL),(34,'qpc3a/ySOTv+2DLRlflfW2ku75XD4OCW','LquX4xefXqMe0bci917+yREMyyu5weW1',NULL,0,'jogador_15@email.com','2017-01-03 11:25:11',1,'2017-01-03 15:01:36',NULL,NULL),(35,'X87f12VN7+7qM/XvdUjIBsmiqEsvrmTZ','hDsVhiblyeL3xadWgNhshPLLU2zFK7I0',NULL,0,'jogador_16@email.com','2017-01-03 11:25:32',1,'2017-01-03 15:01:36',NULL,NULL),(36,'c7SWaL+9xyTmzXRAMI9DVnLg2Vq4xwDf','QPYTmReaXq0zj5Q7WyNwVzoknVVg0sZ6',NULL,0,'jogador_17','2017-01-03 11:26:48',1,'2017-01-03 15:01:36',NULL,NULL),(37,'KugnRLO6FexXYjz6/LUw0x/rBFT2CJDk','fjZBBfNygh5MzKOrlumwObuxzTB9379i',NULL,0,'jogador_18@email.com','2017-01-03 11:27:00',1,'2017-01-03 15:01:36',NULL,NULL),(38,'xbm96qufV++DbT8Wj2IuIjhWLCzlPFba','5TYDdirqdUlo7KINtcNBaviDLIdrgj+V',NULL,0,'jogador_19@email.com','2017-01-04 16:31:19',1,'2017-01-04 16:31:19',NULL,NULL),(39,'PLsXrIQ9PwLATqMjfAdWg3mRRWKaQjEh','yuzQbhtuUFI95UcGVUBskKMuQ8wWWH8N',NULL,0,'teste1','2017-01-09 16:37:51',1,'0001-01-01 00:00:00',NULL,NULL),(40,'HnNlm4pOAQzsmowYQWvybkFg6b7mB/GN','DSDK3jesO2aOFYB7m/mMhzDklxNg5Uio',NULL,0,'1','2017-01-09 16:53:07',1,'2017-01-09 16:53:07',NULL,NULL),(41,'+k6glXiOyyHiWcXPzxkdxKe4qUAdgv6+','ArkLm+0Q6M/w581EM2ZI8eVytljwAH2g',NULL,0,'marcosti','2017-01-09 16:58:52',1,'2017-01-09 16:58:52',NULL,NULL),(44,'qn9VtdKB9TXJwpvLlSry06sqkzh07Url','zlwFKfodykq1MYtH98ujJabj6kp/Di/g',NULL,0,'fabiana@pelegrini.com.br','2017-01-12 19:46:26',1,'2017-01-12 19:46:26',NULL,NULL),(46,'1Mq9NiwbpjE16qvwD5g6ThyQWj/V0OVy','lD7DGPh2SUJHG0bx/Mf1NBZ84BTKasCn',NULL,0,'igor@gamific.com.br','2017-01-11 13:08:26',1,'2017-01-11 13:08:26',NULL,NULL),(47,'IafNY82l3oyavzLpBKq9ObH0Ew+FdGuu','iR3oMhaWkHX8StzkZrcg1Lg7eRVzC/T3',NULL,0,'miller@planilha.com.br','2017-01-12 13:30:41',1,'0001-01-01 00:00:00',NULL,NULL),(48,'nTW/xv/AJCJ98bYmC8LQlNHPgRUNB9nm','tLO3vhqZn7jxPx9EwyN+cGRxFA4oRrLB',NULL,0,'miller1@planilha.com.br','2017-01-12 16:17:29',1,'0001-01-01 00:00:00',NULL,NULL),(49,'BGp1k0XxPYMtrteb1/KrT0rdZJzc/CME','IUe+ugbDQp7sSfX6jEDwL1vmWk99KO38',NULL,0,'miller5@planilha.com.br','2017-01-12 16:50:09',1,'0001-01-01 00:00:00',NULL,NULL),(50,'+yiBugig5lnVZN0o00zZ5KRjr+0qilp1','3r60e6VBpsxpmAd8ZGG0kfTEZ4NOWQFs',NULL,0,'miller7@planilha.com.br','2017-01-12 16:55:48',1,'0001-01-01 00:00:00',NULL,NULL),(51,'AGsfUhF+1qDwd4MBsQimWTk6kdNnsiwZ','VvM/opTbFy1GCd/fJaYZ3pS7H9uzfs6O',NULL,0,'lucas@email.com','2017-01-13 23:57:17',1,'0001-01-01 00:00:00',NULL,NULL);
/*!40000 ALTER TABLE `Account_UserAccount` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `Account_UserProfile`
--

DROP TABLE IF EXISTS `Account_UserProfile`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!40101 SET character_set_client = utf8 */;
CREATE TABLE `Account_UserProfile` (
  `Id` int(11) NOT NULL,
  `Name` varchar(100) NOT NULL,
  `Email` varchar(100) NOT NULL,
  `CPF` varchar(20) DEFAULT NULL,
  `Phone` varchar(20) NOT NULL,
  `LastUpdate` datetime NOT NULL,
  PRIMARY KEY (`Id`)
) ENGINE=InnoDB DEFAULT CHARSET=latin1;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `Account_UserProfile`
--

LOCK TABLES `Account_UserProfile` WRITE;
/*!40000 ALTER TABLE `Account_UserProfile` DISABLE KEYS */;
INSERT INTO `Account_UserProfile` VALUES (1,'Marcos Vinícius de Aguiar Peixoto','marcosaguiar@live.com','11694739627','34991036730','2016-10-05 00:00:00'),(2,'Paulo','paulo@duplov.com.br',NULL,'(34) 32352 - 119','2016-12-05 12:28:46'),(3,'adm','adm@email.com.br',NULL,'(32) 13123 - 2132','0001-01-01 00:00:00'),(4,'Vendedor 1','vendedor1@email.com.br','11694739627','(32) 12312 - 3132','2016-12-05 13:38:07'),(5,'Supervisor 1','supervisor1@email.com.br','11694739627','(13) 12312 - 3123','2016-12-05 13:38:39'),(6,'Vendedor 2','vendedor2@email.com.br','11694739627','(23) 21321 - 3213','2016-12-05 17:33:39'),(7,'Gerente 1','gerente1@email.com.br','11694739627','(23) 12321 - 3213','2016-12-05 17:34:07'),(8,'Priscilla Borges Felipe','priscilla@duplov.com.br','073.544.386-69','(34) 99828 - 1333','0001-01-01 00:00:00'),(9,'Guilherme Portilho Porto','guilherme@duplov.com.br','033.548.211-24','(34) 99839 - 0414','0001-01-01 00:00:00'),(10,'Hugo de Faria Lopes','hugo@duplov.com.br','061.832.116-05','(34) 98801 - 5273','0001-01-01 00:00:00'),(11,'Hugo de Faria Lopes','hugoadm@duplov.com.br','061.832.116-05','(34) 98801 - 5273','0001-01-01 00:00:00'),(12,'Glaucielly Aparecida de Paiva','glaucielly@duplov.com.br','83010726104','(34) 99128 - 4048','2016-12-06 11:23:14'),(13,'Alexsander Marçal','alexsander@duplov.com.br','11694739627','(34) 99261 - 6640','2016-12-06 11:24:32'),(14,'Caio Bernardes Pupatto','caio@duplov.com.br','10793378656','(34) 99694 - 9241','2016-12-06 11:25:17'),(15,'Gustavo Martins Carvalho','gustavo@duplov.com.br','08579081629','(34) 99318 - 0438','2016-12-06 11:25:58'),(16,'Gustavo Martins Carvalho','gustavoadm@duplov.com.br','08579081629','(34) 99318 - 0438','2016-12-06 11:26:41'),(17,'Rubens samuel de Melo','rubens@duplov.com.br','05408818640','(32) 12312 - 3132','2016-12-06 13:27:05'),(18,'Flávio santos Pereira','flavio@duplov.com.br','08058720607','(32) 12312 - 3132','2016-12-06 16:36:33'),(20,'Igor','igorgarantes@gmail.com','12120942633','(34) 99191 - 3113','0001-01-01 00:00:00'),(21,'Rubens','rubs@email.com','11111111111','(99) 99999 - 999','2016-12-19 13:38:30'),(22,'miller','miller@gamific.com.br','06345959626','(34) 99136 - 5434','0001-01-01 00:00:00'),(23,'engine','engine@email.com','11111111111','(99) 99999 - 9999','0001-01-01 00:00:00'),(24,'mille2','miller2@email.com','11111111111','(99) 99999 - 9999','0001-01-01 00:00:00'),(25,'miller3','miller3@email.com','11111111111','(99) 99999 - 9999','0001-01-01 00:00:00'),(26,'Igor6','e','11111111111','(99) 99999 - 9999','0001-01-01 00:00:00'),(27,'funcionario teste 55','isaodsa@dasd.com.br','11694739627','(13) 12321 - 3213','0001-01-01 00:00:00'),(28,'igor23','igor23@email.com','11111111111','(99) 99999 - 9999','2016-12-27 17:12:29'),(29,'igor12','igor12@email.com','11111111111','(99) 99999 - 9999','2016-12-27 17:22:57'),(30,'Jogador_11','jogador_11@email.com','11111111111','(99) 99999 - 9999','0001-01-01 00:00:00'),(31,'Jogador_12','jogador_12@email.com','11111111111','(99) 99999 - 9999','0001-01-01 00:00:00'),(32,'Jogador_13','jogador_13@email.com','11111111111','(99) 99999 - 9999','0001-01-01 00:00:00'),(33,'Jogador_14','jogador_14@email.com','11111111111','(99) 99999 - 9999','0001-01-01 00:00:00'),(34,'Jogador_15','jogador_15@email.com','11111111111','(99) 99999 - 9999','0001-01-01 00:00:00'),(35,'Jogador_16','jogador_16@email.com','11111111111','(99) 99999 - 9999','0001-01-01 00:00:00'),(36,'Jogador_17','jogador_17','11111111111','(99) 99999 - 9999','0001-01-01 00:00:00'),(37,'Jogador_18','jogador_18@email.com','11111111111','(99) 99999 - 9999','0001-01-01 00:00:00'),(38,'Jogador_19','jogador_19@email.com','11111111111','(99) 99999 - 9999','0001-01-01 00:00:00'),(39,'teste','teste@email.com',NULL,'(99) 99999 - 9999','2017-01-09 16:37:52'),(40,'1','1',NULL,'(1','2017-01-09 16:51:11'),(41,'Marcos TI','marcos.gomes@tisuporte.info',NULL,'(34) 99210 - 9701','2017-01-09 16:56:56'),(44,'Fabiana','fabiana@pelegrini.com.br','11111111111','(34) 99979 - 7673','0001-01-01 00:00:00'),(46,'Igor','igor@gamific.com.br','11111111111','(99) 99999 - 9999','0001-01-01 00:00:00'),(47,'Miller Miranda Planilha','miller@planilha.com.br','11694739627','34991036730','2017-01-12 13:30:41'),(48,'Miller Miranda Planilha 1','miller1@planilha.com.br','11694739627','34991036750','2017-01-12 16:17:29'),(49,'Miller Miranda Planilha 5','miller5@planilha.com.br','11694739627','34991536750','2017-01-12 16:50:09'),(50,'Miller Miranda Planilha 7','miller7@planilha.com.br','11694739627','34991538750','2017-01-12 16:55:48'),(51,'Lucas','lucas@email.com','11111111111','(34) 99999 - 9999','2017-01-13 23:57:17');
/*!40000 ALTER TABLE `Account_UserProfile` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `Account_UserRole`
--

DROP TABLE IF EXISTS `Account_UserRole`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!40101 SET character_set_client = utf8 */;
CREATE TABLE `Account_UserRole` (
  `UserId` int(11) NOT NULL,
  `Role` int(11) NOT NULL,
  PRIMARY KEY (`UserId`,`Role`),
  KEY `Role_FK_idx` (`Role`),
  CONSTRAINT `UserRole_Account_FK` FOREIGN KEY (`UserId`) REFERENCES `Account_UserAccount` (`Id`) ON DELETE NO ACTION ON UPDATE NO ACTION
) ENGINE=InnoDB DEFAULT CHARSET=latin1;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `Account_UserRole`
--

LOCK TABLES `Account_UserRole` WRITE;
/*!40000 ALTER TABLE `Account_UserRole` DISABLE KEYS */;
INSERT INTO `Account_UserRole` VALUES (1,0),(2,1),(3,1),(4,1),(5,1),(6,1),(7,1),(8,1),(9,1),(10,1),(11,1),(12,1),(13,1),(14,1),(15,1),(16,1),(17,1),(18,1),(20,1),(21,1),(22,1),(23,1),(24,1),(25,1),(26,1),(27,1),(28,1),(29,1),(30,1),(31,1),(32,1),(33,1),(34,1),(35,1),(36,1),(37,1),(38,1),(39,1),(40,1),(41,1),(44,1),(46,1),(47,1),(48,1),(49,1),(50,1),(51,1);
/*!40000 ALTER TABLE `Account_UserRole` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `Firm_Campaign`
--

DROP TABLE IF EXISTS `Firm_Campaign`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!40101 SET character_set_client = utf8 */;
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
  CONSTRAINT `SPONSOR_ID_CAMPAIGN` FOREIGN KEY (`CreatedBy`) REFERENCES `Firm_Worker` (`Id`) ON DELETE NO ACTION ON UPDATE NO ACTION,
  CONSTRAINT `WORKER_TYPE_ID_CAMPAIGN` FOREIGN KEY (`WorkerTypeId`) REFERENCES `Firm_Worker_Type` (`Id`) ON DELETE NO ACTION ON UPDATE NO ACTION
) ENGINE=InnoDB DEFAULT CHARSET=latin1;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `Firm_Campaign`
--

LOCK TABLES `Firm_Campaign` WRITE;
/*!40000 ALTER TABLE `Firm_Campaign` DISABLE KEYS */;
/*!40000 ALTER TABLE `Firm_Campaign` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `Firm_Data`
--

DROP TABLE IF EXISTS `Firm_Data`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!40101 SET character_set_client = utf8 */;
CREATE TABLE `Firm_Data` (
  `Id` int(11) NOT NULL AUTO_INCREMENT,
  `FirmName` varchar(100) NOT NULL,
  `CompanyName` varchar(100) NOT NULL,
  `CNPJ` varchar(100) NOT NULL,
  `LogoId` int(11) DEFAULT '0',
  `Adress` varchar(100) NOT NULL,
  `Neighborhood` varchar(100) NOT NULL,
  `City` varchar(100) NOT NULL,
  `UpdatedBy` int(11) NOT NULL,
  `Status` tinyint(4) NOT NULL,
  `LastUpdate` datetime NOT NULL,
  `Phone` varchar(30) NOT NULL,
  `ExternalId` varchar(25) NOT NULL,
  PRIMARY KEY (`Id`)
) ENGINE=InnoDB AUTO_INCREMENT=8 DEFAULT CHARSET=latin1;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `Firm_Data`
--

LOCK TABLES `Firm_Data` WRITE;
/*!40000 ALTER TABLE `Firm_Data` DISABLE KEYS */;
INSERT INTO `Firm_Data` VALUES (1,'Duplov','Duplov','11.111.111/1111-11',1,'Rua tapuios, número 828','Saraiva','Uberlândia',1,1,'2016-12-05 12:28:47','(34) 32352 - 119',''),(2,'Microsoft','Microsoft','00.000.000/0000-00',2,'Rua teste','Bairro teste','Cidade Teste',1,1,'2016-12-05 13:04:26','(12) 31232 - 1321','58615a403a87781fbdea1bf4'),(3,'Teste','Teste','11.111.111/1111-11',0,'Endereco','Bairro','Cidade',1,1,'2017-01-09 16:37:52','(99) 99999 - 9999','5873bc5e3a87781f8fdf4d5b'),(4,'1','1','1',0,'1','1','1',1,1,'2017-01-09 16:51:21','(1','5873bf863a87781f8fdf4d5c'),(5,'Forca P Teste','Forca P Teste','11.111.111/1111-11',0,'Rua Tapuios','Saraiva','Uberlandia',1,1,'2017-01-09 16:56:57','(34) 99210 - 9701','5873c0d83a87781f8fdf4d5d'),(6,'ForcaP Internacional','ForcaP Internacional','11.111.111/1111-11',47,'R. Dr. Luiz Antonio Waack','Marta Helena','Uberlandia',1,1,'2017-01-11 11:10:05','(34) 32918 - 825','587610753a87782347bda33c'),(7,'Igor Enterprise ç','Igor Enterprise ç','11.111.111/1111-11',54,'Tapuios','Saraiva','Uberlândia',1,1,'2017-01-11 12:27:13','(99) 99999 - 9999','58761d123a87782347bda33e');
/*!40000 ALTER TABLE `Firm_Data` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `Firm_Goal`
--

DROP TABLE IF EXISTS `Firm_Goal`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!40101 SET character_set_client = utf8 */;
CREATE TABLE `Firm_Goal` (
  `Id` int(11) NOT NULL AUTO_INCREMENT,
  `ExternalMetricId` varchar(25) NOT NULL,
  `EpisodeId` varchar(25) NOT NULL,
  `RunId` varchar(25) NOT NULL,
  `Goal` int(11) NOT NULL,
  `UpdatedBy` int(11) NOT NULL,
  `LastUpdate` datetime NOT NULL,
  PRIMARY KEY (`Id`)
) ENGINE=InnoDB AUTO_INCREMENT=98 DEFAULT CHARSET=latin1;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `Firm_Goal`
--

LOCK TABLES `Firm_Goal` WRITE;
/*!40000 ALTER TABLE `Firm_Goal` DISABLE KEYS */;
INSERT INTO `Firm_Goal` VALUES (94,'5873cb8d3a87781f8fdf4d5e','586b88e53a877862cb6330df','587372643a87781eee541bbb',16,3,'2017-01-11 16:37:21'),(95,'5873cb8d3a87781f8fdf4d5e','586b88e53a877862cb6330df','58767a193a87782347bda349',54,3,'2017-01-12 16:02:06'),(96,'5873cb8d3a87781f8fdf4d5e','','58767a193a87782347bda34a',2066,3,'2017-01-12 18:49:51'),(97,'5873cb8d3a87781f8fdf4d5e','586b88e53a877862cb6330df','586bdb193a877862cb63314f',14,3,'2017-01-12 16:01:32');
/*!40000 ALTER TABLE `Firm_Goal` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `Firm_Message`
--

DROP TABLE IF EXISTS `Firm_Message`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!40101 SET character_set_client = utf8 */;
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
) ENGINE=InnoDB AUTO_INCREMENT=47 DEFAULT CHARSET=latin1;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `Firm_Message`
--

LOCK TABLES `Firm_Message` WRITE;
/*!40000 ALTER TABLE `Firm_Message` DISABLE KEYS */;
INSERT INTO `Firm_Message` VALUES (1,1,10,3,'2016-12-08 13:46:53','Bom dia pessoal.'),(2,2,20,6,'2016-12-19 13:43:10','Fala Miller, vamos aumentar esse backlog ae que ta pequeno!'),(3,2,22,6,'2016-12-19 13:46:01','Vamos trabalhar sabado, domingo.\r\n'),(4,2,20,6,'2016-12-19 13:46:03','uma ae'),(5,2,20,6,'2016-12-19 13:56:09','teste'),(6,2,20,6,'2016-12-19 13:59:55','te'),(7,2,20,6,'2016-12-19 14:00:04','teste'),(8,2,20,6,'2016-12-19 16:20:00','Mais um teste'),(9,2,20,6,'2016-12-19 16:22:45','teete'),(10,2,20,6,'2016-12-19 16:55:33','testeee'),(11,2,20,6,'2016-12-19 17:05:15','mandando uma mensagem'),(12,2,20,6,'2016-12-19 17:07:00','enviando...'),(13,2,20,6,'2016-12-19 17:07:11','enviando...'),(14,2,20,6,'2016-12-19 17:10:43','enviando'),(15,2,20,6,'2016-12-19 17:12:51','mensagem'),(16,2,20,6,'2016-12-19 17:18:27','teste'),(17,2,20,6,'2016-12-19 17:19:40','msg'),(18,2,20,6,'2016-12-19 17:22:19','mensageeeemmm'),(19,2,20,6,'2016-12-19 17:23:31','teste'),(20,2,20,6,'2016-12-19 17:26:24','teste'),(21,2,20,6,'2016-12-19 17:27:23','miller\n'),(22,2,20,6,'2016-12-19 17:29:35','fasdfsf\n'),(23,2,20,6,'2016-12-19 17:42:02','teste'),(24,2,22,6,'2016-12-19 17:47:44','Ficou show.'),(25,2,20,6,'2016-12-19 17:59:16','teste'),(26,2,20,6,'2016-12-19 18:03:10','Enviando mensagem \n'),(27,2,20,6,'2016-12-20 10:38:03','mais um '),(28,2,20,6,'2016-12-20 10:38:16','teste'),(29,2,21,6,'2016-12-20 11:01:57','Fala pessoal!'),(30,2,3,1,'2016-12-27 14:04:58','oi'),(31,2,3,1,'2016-12-27 13:09:31','ola\n'),(32,2,3,1,'2016-12-27 14:06:51','dsdsfsdfsdfs'),(33,2,3,1,'2016-12-27 13:11:47','teste'),(34,2,3,1,'2016-12-27 13:38:38','teste'),(35,2,3,1,'2016-12-27 13:40:31','teste12'),(36,2,3,1,'2016-12-27 14:59:35','ola'),(37,2,3,1,'2016-12-27 14:59:48','teste fim'),(38,2,3,1,'2016-12-27 15:03:01','hgf'),(39,2,22,6,'2016-12-27 15:10:11','teste'),(40,2,3,6,'2016-12-27 15:16:41','teste '),(41,2,3,6,'2016-12-27 15:16:43','teste '),(42,2,22,6,'2016-12-27 15:17:52','nova mensagem dia 27-12-2016\n'),(43,2,3,6,'2016-12-27 15:18:17','aff'),(44,2,3,6,'2016-12-27 15:19:59','gfdgd'),(45,2,3,6,'2016-12-29 08:43:40','teste'),(46,2,3,6,'2016-12-30 20:24:40','ghjghj');
/*!40000 ALTER TABLE `Firm_Message` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `Firm_Metric`
--

DROP TABLE IF EXISTS `Firm_Metric`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!40101 SET character_set_client = utf8 */;
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
  `ExternalID` varchar(200) DEFAULT NULL,
  PRIMARY KEY (`Id`),
  KEY `FIRM_ID_METRIC` (`FirmId`),
  CONSTRAINT `FIRM_ID_METRIC` FOREIGN KEY (`FirmId`) REFERENCES `Firm_Data` (`Id`) ON DELETE NO ACTION ON UPDATE NO ACTION
) ENGINE=InnoDB AUTO_INCREMENT=17 DEFAULT CHARSET=latin1;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `Firm_Metric`
--

LOCK TABLES `Firm_Metric` WRITE;
/*!40000 ALTER TABLE `Firm_Metric` DISABLE KEYS */;
INSERT INTO `Firm_Metric` VALUES (1,'Visitas porta a porta',1,2,'2016-12-05 12:30:31',1,'fa_users',10,10,40,NULL),(2,'Primeira Reunião',1,2,'2016-12-05 12:32:03',1,'fa_users',10,10,40,NULL),(3,'Venda',1,2,'2016-12-05 12:32:31',1,'fa_usd',NULL,1,NULL,NULL),(4,'Contrato 12 meses',1,2,'2016-12-05 12:32:54',1,'fa_usd',NULL,100,NULL,NULL),(5,'Indicação',1,2,'2016-12-05 12:33:36',1,'fa_phone',20,1,200,NULL),(6,'Indicação fechada',1,2,'2016-12-05 12:34:03',1,'fa_phone',NULL,10,NULL,NULL),(7,'Novos Clientes',1,2,'2016-12-05 12:34:28',1,'fa_users',NULL,100,NULL,NULL),(8,'Hotsite',1,2,'2016-12-05 12:34:49',1,'fa_usd',NULL,200,NULL,NULL),(9,'Vendas',2,3,'2016-12-05 18:53:17',1,'fa_users',10,3,20,NULL),(10,'Bilho 2',2,3,'2016-12-23 02:43:17',0,'fa_bar_chart_o',4,3,8,'585c89ce3a87781d62b634e9'),(11,'tet',2,3,'2016-12-23 02:44:19',0,'fa_lightbulb_o',1,1,1,'585c8f5e3a87781d62b634ea'),(12,'tet',2,3,'2016-12-23 02:45:57',0,'fa_lightbulb_o',1,1,1,'585c8f5f3a87781d62b634eb'),(13,'Teste',2,3,'2016-12-23 02:50:55',0,'fa_users',1,1,1,'585c91003a87781d62b634ec'),(14,'te',2,3,'2016-12-23 03:02:31',0,'fa_phone',1,1,1,'585c937f3a87781d62b634ed'),(15,'Bilho Lindu',2,3,'2016-12-23 03:13:57',1,'fa_phone',1,1,1,'585c96753a87781d62b634ee'),(16,'3',2,3,'2016-12-23 03:14:25',0,'fa_users',3,3,3,'585c96863a87781d62b634ef');
/*!40000 ALTER TABLE `Firm_Metric` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `Firm_Param`
--

DROP TABLE IF EXISTS `Firm_Param`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!40101 SET character_set_client = utf8 */;
CREATE TABLE `Firm_Param` (
  `Id` int(11) unsigned NOT NULL AUTO_INCREMENT,
  `Name` varchar(100) NOT NULL,
  `Value` varchar(100) NOT NULL,
  `Description` varchar(500) NOT NULL,
  `GameId` varchar(25) NOT NULL,
  `LastUpdate` datetime NOT NULL,
  `UpdateBy` varchar(100) NOT NULL,
  PRIMARY KEY (`Id`),
  UNIQUE KEY `uniqueName` (`Name`,`GameId`),
  KEY `external_id` (`GameId`)
) ENGINE=InnoDB AUTO_INCREMENT=19 DEFAULT CHARSET=latin1;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `Firm_Param`
--

LOCK TABLES `Firm_Param` WRITE;
/*!40000 ALTER TABLE `Firm_Param` DISABLE KEYS */;
INSERT INTO `Firm_Param` VALUES (6,'Parametro 1','12','teste','58615a403a87781fbdea1bf4','2017-01-16 12:27:47','3'),(15,'t5','teste','aqui vem uma descrição','58615a403a87781fbdea1bf4','2017-01-16 13:49:21','3'),(16,'test 98','kn','jhuvuvuj','58615a403a87781fbdea1bf4','2017-01-16 14:23:46','3'),(17,'teste 2','test','At vero eos et accusamus et iusto odio dignissimos ducimus qui blanditiis praesentium voluptatum deleniti atque corrupti quos dolores et quas molestias excepturi sint occaecati cupiditate non provident, similique sunt in culpa qui officia deserunt mollitia animi, id est laborum et dolorum fuga. Et harum quidem rerum facilis est et expedita distinctio. Nam libero tempore, cum soluta nobis est eligendi optio cumque nihil impedit quo minus id quod maxime placeat facere possimus, omnis voluptas assu','58615a403a87781fbdea1bf4','2017-01-16 14:39:31','3'),(18,'teste','teste','teste','58615a403a87781fbdea1bf4','2017-01-16 15:20:49','3');
/*!40000 ALTER TABLE `Firm_Param` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `Firm_Player_Run`
--

DROP TABLE IF EXISTS `Firm_Player_Run`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!40101 SET character_set_client = utf8 */;
CREATE TABLE `Firm_Player_Run` (
  `Id` int(11) NOT NULL AUTO_INCREMENT,
  `WorkerId` int(11) NOT NULL,
  `ExternalTeamId` varchar(100) NOT NULL,
  `EpisodeId` varchar(100) NOT NULL,
  `RunId` varchar(100) NOT NULL,
  `UpdatedBy` int(11) NOT NULL,
  `Status` tinyint(4) NOT NULL,
  PRIMARY KEY (`Id`),
  KEY `WORKER_ID_PLAYER_RUN` (`WorkerId`),
  CONSTRAINT `WORKER_ID_PLAYER_RUN` FOREIGN KEY (`WorkerId`) REFERENCES `Firm_Worker` (`Id`) ON DELETE NO ACTION ON UPDATE NO ACTION
) ENGINE=InnoDB AUTO_INCREMENT=3 DEFAULT CHARSET=latin1;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `Firm_Player_Run`
--

LOCK TABLES `Firm_Player_Run` WRITE;
/*!40000 ALTER TABLE `Firm_Player_Run` DISABLE KEYS */;
INSERT INTO `Firm_Player_Run` VALUES (1,3,'5850a6c13a87781d39875588','5850a6c13a87781d39875587','5850a6c13a87781d398755c5',1,1),(2,5,'5850a6c13a87781d39875588','5850a6c13a87781d39875587','5850a6c13a87781d398755c0',1,1);
/*!40000 ALTER TABLE `Firm_Player_Run` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `Firm_Result`
--

DROP TABLE IF EXISTS `Firm_Result`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!40101 SET character_set_client = utf8 */;
CREATE TABLE `Firm_Result` (
  `Id` int(11) NOT NULL AUTO_INCREMENT,
  `FirmId` int(11) NOT NULL,
  `WorkerId` int(11) NOT NULL,
  `MetricId` int(11) NOT NULL,
  `Period` datetime NOT NULL,
  `UpdatedBy` int(11) NOT NULL,
  `LastUpdate` datetime NOT NULL,
  `Result` int(11) NOT NULL,
  `MainResult` int(11) DEFAULT NULL,
  PRIMARY KEY (`Id`),
  KEY `FIRM_ID_RESULT` (`FirmId`),
  KEY `WORKER_ID_RESULT` (`WorkerId`),
  KEY `METRIC_ID_RESULT` (`MetricId`),
  KEY `MainResult` (`MainResult`),
  CONSTRAINT `FIRM_ID_RESULT` FOREIGN KEY (`FirmId`) REFERENCES `Firm_Data` (`Id`) ON DELETE NO ACTION ON UPDATE NO ACTION,
  CONSTRAINT `Firm_Result_ibfk_1` FOREIGN KEY (`MainResult`) REFERENCES `Firm_Result` (`Id`),
  CONSTRAINT `METRIC_ID_RESULT` FOREIGN KEY (`MetricId`) REFERENCES `Firm_Metric` (`Id`) ON DELETE NO ACTION ON UPDATE NO ACTION,
  CONSTRAINT `WORKER_ID_RESULT` FOREIGN KEY (`WorkerId`) REFERENCES `Firm_Worker` (`Id`) ON DELETE NO ACTION ON UPDATE NO ACTION
) ENGINE=InnoDB AUTO_INCREMENT=7 DEFAULT CHARSET=latin1;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `Firm_Result`
--

LOCK TABLES `Firm_Result` WRITE;
/*!40000 ALTER TABLE `Firm_Result` DISABLE KEYS */;
INSERT INTO `Firm_Result` VALUES (1,2,3,9,'2016-12-14 00:00:00',0,'2016-12-05 19:09:22',10,NULL),(2,2,5,9,'2016-12-05 19:34:24',3,'2016-12-05 19:34:30',3,NULL),(3,2,3,9,'2016-12-05 19:34:52',3,'2016-12-05 19:34:59',3,NULL),(4,2,3,9,'2016-12-06 13:22:30',3,'2016-12-06 15:22:37',4,NULL),(5,2,4,9,'2016-12-06 13:22:30',3,'2016-12-06 15:22:38',4,4),(6,2,6,9,'2016-12-06 13:22:30',3,'2016-12-06 15:22:38',4,4);
/*!40000 ALTER TABLE `Firm_Result` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `Firm_Team`
--

DROP TABLE IF EXISTS `Firm_Team`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!40101 SET character_set_client = utf8 */;
CREATE TABLE `Firm_Team` (
  `Id` int(11) NOT NULL AUTO_INCREMENT,
  `TeamName` varchar(50) NOT NULL,
  `FirmId` int(11) NOT NULL,
  `SponsorId` int(11) NOT NULL,
  `WorkerTypeId` int(11) NOT NULL,
  `UpdatedBy` int(11) NOT NULL,
  `Status` tinyint(4) NOT NULL,
  `LogoId` int(11) DEFAULT '0',
  `LastUpdate` datetime NOT NULL,
  `ExternalId` varchar(25) DEFAULT NULL,
  PRIMARY KEY (`Id`),
  UNIQUE KEY `IdExterno` (`ExternalId`),
  KEY `FIRM_ID_TEAM` (`FirmId`),
  KEY `SPONSOR_ID_TEAM` (`SponsorId`),
  KEY `WORKER_TYPE_ID_TEAM` (`WorkerTypeId`),
  CONSTRAINT `FIRM_ID_TEAM` FOREIGN KEY (`FirmId`) REFERENCES `Firm_Data` (`Id`) ON DELETE NO ACTION ON UPDATE NO ACTION,
  CONSTRAINT `SPONSOR_ID_TEAM` FOREIGN KEY (`SponsorId`) REFERENCES `Firm_Worker` (`Id`) ON DELETE NO ACTION ON UPDATE NO ACTION,
  CONSTRAINT `WORKER_TYPE_ID_TEAM` FOREIGN KEY (`WorkerTypeId`) REFERENCES `Firm_Worker_Type` (`Id`) ON DELETE NO ACTION ON UPDATE NO ACTION
) ENGINE=InnoDB AUTO_INCREMENT=10 DEFAULT CHARSET=latin1;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `Firm_Team`
--

LOCK TABLES `Firm_Team` WRITE;
/*!40000 ALTER TABLE `Firm_Team` DISABLE KEYS */;
INSERT INTO `Firm_Team` VALUES (1,'Equipe Vendedores',2,4,10,3,1,6,'2016-12-05 13:48:56','5850a6c13a87781d39875588'),(2,'Equipe de supervisores',2,6,7,3,1,9,'2016-12-05 17:37:05','5850a6c13a87781d3987550d'),(3,'Equipe comercial A',1,10,3,2,1,20,'2016-12-06 13:27:26',NULL),(4,'Equipe comercial B',1,15,3,2,1,21,'2016-12-06 13:27:41',NULL),(5,'Equipe de supervisores',1,16,4,2,1,22,'2016-12-06 13:27:59',NULL),(6,'Equipe Gamific',2,19,10,3,1,0,'2016-12-19 13:39:55','5850a6c13a87781d3987550e'),(7,'Equipe teste1',2,21,10,3,1,0,'2016-12-26 20:06:35','5850a6c13a87781d398755ed'),(8,'Equipe teste 2',2,23,10,3,1,0,'2016-12-26 20:06:51','5853d2c83a87781d62b634a7'),(9,'Equipe Sipat 2016',2,25,10,3,1,0,'2016-12-26 20:08:11','585bb8f93a87781d62b634e1');
/*!40000 ALTER TABLE `Firm_Team` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `Firm_Team_Worker`
--

DROP TABLE IF EXISTS `Firm_Team_Worker`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!40101 SET character_set_client = utf8 */;
CREATE TABLE `Firm_Team_Worker` (
  `Id` int(11) NOT NULL AUTO_INCREMENT,
  `ExternalTeamId` varchar(25) NOT NULL,
  `ExternalWorkerId` varchar(25) NOT NULL,
  `FirmId` int(11) NOT NULL,
  `TeamId` int(11) DEFAULT NULL,
  `WorkerId` int(11) DEFAULT NULL,
  `UpdatedBy` int(11) NOT NULL,
  `LastUpdate` datetime NOT NULL,
  `Status` tinyint(4) NOT NULL,
  PRIMARY KEY (`Id`),
  KEY `FIRM_ID_TEAM_WORKER` (`FirmId`),
  CONSTRAINT `FIRM_ID_TEAM_WORKER` FOREIGN KEY (`FirmId`) REFERENCES `Firm_Data` (`Id`) ON DELETE NO ACTION ON UPDATE NO ACTION
) ENGINE=InnoDB AUTO_INCREMENT=18 DEFAULT CHARSET=latin1;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `Firm_Team_Worker`
--

LOCK TABLES `Firm_Team_Worker` WRITE;
/*!40000 ALTER TABLE `Firm_Team_Worker` DISABLE KEYS */;
INSERT INTO `Firm_Team_Worker` VALUES (1,'','',2,1,3,3,'2016-12-05 13:54:25',1),(2,'','',2,2,4,3,'2016-12-05 17:37:23',1),(3,'','',1,3,7,2,'2016-12-06 13:28:26',1),(4,'','',1,3,8,2,'2016-12-06 13:28:26',1),(5,'','',1,3,9,2,'2016-12-06 13:28:26',1),(6,'','',1,4,11,2,'2016-12-06 13:28:34',1),(7,'','',1,4,12,2,'2016-12-06 13:28:34',1),(8,'','',1,4,13,2,'2016-12-22 18:22:50',1),(9,'','',1,4,14,2,'2016-12-06 13:28:34',1),(10,'','',1,5,10,2,'2016-12-06 13:28:42',1),(11,'','',1,5,15,2,'2016-12-06 13:28:42',1),(12,'','',1,4,17,2,'2016-12-06 16:38:17',1),(13,'','',2,6,18,3,'2016-12-19 13:40:47',1),(14,'','',2,6,20,3,'2016-12-19 13:40:47',1),(15,'','',2,2,19,3,'2016-12-26 18:34:24',1),(16,'','',2,1,5,3,'2016-12-26 18:34:32',1),(17,'58664c173a8778461003500a','5850a6c13a87781d39875510',2,NULL,NULL,3,'2017-01-02 10:58:58',1);
/*!40000 ALTER TABLE `Firm_Team_Worker` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `Firm_Video`
--

DROP TABLE IF EXISTS `Firm_Video`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!40101 SET character_set_client = utf8 */;
CREATE TABLE `Firm_Video` (
  `Id` int(11) NOT NULL AUTO_INCREMENT,
  `VideoUrl` text NOT NULL,
  `FirmId` int(11) NOT NULL,
  `VideoTitle` varchar(100) NOT NULL,
  `Status` tinyint(4) NOT NULL,
  `UpdatedBy` int(11) NOT NULL,
  `LastUpdate` datetime NOT NULL,
  PRIMARY KEY (`Id`)
) ENGINE=InnoDB DEFAULT CHARSET=latin1;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `Firm_Video`
--

LOCK TABLES `Firm_Video` WRITE;
/*!40000 ALTER TABLE `Firm_Video` DISABLE KEYS */;
/*!40000 ALTER TABLE `Firm_Video` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `Firm_Video_Question`
--

DROP TABLE IF EXISTS `Firm_Video_Question`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!40101 SET character_set_client = utf8 */;
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
) ENGINE=InnoDB DEFAULT CHARSET=latin1;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `Firm_Video_Question`
--

LOCK TABLES `Firm_Video_Question` WRITE;
/*!40000 ALTER TABLE `Firm_Video_Question` DISABLE KEYS */;
/*!40000 ALTER TABLE `Firm_Video_Question` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `Firm_Video_Question_Answered`
--

DROP TABLE IF EXISTS `Firm_Video_Question_Answered`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!40101 SET character_set_client = utf8 */;
CREATE TABLE `Firm_Video_Question_Answered` (
  `Id` int(11) NOT NULL AUTO_INCREMENT,
  `VideoQuestionId` int(11) NOT NULL,
  `UserId` int(11) NOT NULL,
  `Answer` text NOT NULL,
  `AnsweredDate` datetime NOT NULL,
  PRIMARY KEY (`Id`),
  KEY `VIDEO_QUESTION_ID` (`VideoQuestionId`),
  KEY `USER_ID_VIDEO_QUESTION_ANSWERED` (`UserId`),
  CONSTRAINT `USER_ID_VIDEO_QUESTION_ANSWERED` FOREIGN KEY (`UserId`) REFERENCES `Account_UserAccount` (`Id`) ON DELETE NO ACTION ON UPDATE NO ACTION,
  CONSTRAINT `VIDEO_QUESTION_ID` FOREIGN KEY (`VideoQuestionId`) REFERENCES `Firm_Video_Question` (`Id`) ON DELETE NO ACTION ON UPDATE NO ACTION
) ENGINE=InnoDB DEFAULT CHARSET=latin1;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `Firm_Video_Question_Answered`
--

LOCK TABLES `Firm_Video_Question_Answered` WRITE;
/*!40000 ALTER TABLE `Firm_Video_Question_Answered` DISABLE KEYS */;
/*!40000 ALTER TABLE `Firm_Video_Question_Answered` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `Firm_Worker`
--

DROP TABLE IF EXISTS `Firm_Worker`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!40101 SET character_set_client = utf8 */;
CREATE TABLE `Firm_Worker` (
  `Id` int(11) NOT NULL AUTO_INCREMENT,
  `WorkerTypeId` int(11) NOT NULL,
  `UserId` int(11) NOT NULL,
  `FirmId` int(11) NOT NULL,
  `UpdatedBy` int(11) NOT NULL,
  `LogoId` int(11) DEFAULT '0',
  `Status` tinyint(4) NOT NULL,
  `LastUpdate` datetime NOT NULL,
  `ExternalId` varchar(25) DEFAULT NULL,
  `ExternalFirmId` varchar(25) DEFAULT NULL,
  PRIMARY KEY (`Id`),
  UNIQUE KEY `IdExterno` (`ExternalId`),
  KEY `FIRM_ID_WORKER` (`FirmId`),
  KEY `USER_ID_WORKER` (`UserId`),
  KEY `WORKER_TYPE_ID_WORKER` (`WorkerTypeId`),
  CONSTRAINT `FIRM_ID_WORKER` FOREIGN KEY (`FirmId`) REFERENCES `Firm_Data` (`Id`) ON DELETE NO ACTION ON UPDATE NO ACTION,
  CONSTRAINT `USER_ID_WORKER` FOREIGN KEY (`UserId`) REFERENCES `Account_UserAccount` (`Id`) ON DELETE NO ACTION ON UPDATE NO ACTION,
  CONSTRAINT `WORKER_TYPE_ID_WORKER` FOREIGN KEY (`WorkerTypeId`) REFERENCES `Firm_Worker_Type` (`Id`) ON DELETE NO ACTION ON UPDATE NO ACTION
) ENGINE=InnoDB AUTO_INCREMENT=47 DEFAULT CHARSET=latin1;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `Firm_Worker`
--

LOCK TABLES `Firm_Worker` WRITE;
/*!40000 ALTER TABLE `Firm_Worker` DISABLE KEYS */;
INSERT INTO `Firm_Worker` VALUES (1,1,2,1,1,1,1,'2016-12-05 12:28:47',NULL,NULL),(2,6,3,2,1,2,1,'2016-12-05 13:04:26',NULL,'58615a403a87781fbdea1bf4'),(3,10,4,2,3,4,1,'2016-12-05 13:38:07','5850a6c13a87781d39875510','58615a403a87781fbdea1bf4'),(4,7,5,2,3,5,1,'2016-12-05 13:38:39',NULL,'58615a403a87781fbdea1bf4'),(5,10,6,2,3,7,1,'2016-12-05 17:33:39','5850a6c13a87781d3987550f','58615a403a87781fbdea1bf4'),(6,8,7,2,3,8,1,'2016-12-05 17:34:07',NULL,'58615a403a87781fbdea1bf4'),(7,3,8,1,2,10,1,'2016-12-06 10:49:51',NULL,NULL),(8,3,9,1,2,11,1,'2016-12-06 10:50:00',NULL,NULL),(9,3,10,1,2,12,1,'2016-12-06 10:50:10',NULL,NULL),(10,4,11,1,2,13,1,'2016-12-06 10:50:25',NULL,NULL),(11,3,12,1,2,14,1,'2016-12-06 11:23:14',NULL,NULL),(12,3,13,1,2,15,1,'2016-12-06 11:24:32',NULL,NULL),(13,3,14,1,2,16,1,'2016-12-06 11:25:17',NULL,NULL),(14,3,15,1,2,17,1,'2016-12-06 11:25:58',NULL,NULL),(15,4,16,1,2,18,1,'2016-12-06 11:26:41',NULL,NULL),(16,5,17,1,2,19,1,'2016-12-06 13:27:05',NULL,NULL),(17,3,18,1,2,23,1,'2016-12-06 16:36:33',NULL,NULL),(18,10,20,2,3,24,1,'2016-12-19 16:16:49',NULL,'58615a403a87781fbdea1bf4'),(19,7,21,2,3,0,1,'2016-12-19 13:38:30',NULL,'58615a403a87781fbdea1bf4'),(20,10,22,2,3,25,1,'2016-12-19 16:17:11',NULL,'58615a403a87781fbdea1bf4'),(21,8,23,2,3,0,1,'2016-12-22 16:44:59',NULL,'58615a403a87781fbdea1bf4'),(22,9,24,2,3,0,0,'2016-12-22 18:56:05','585c0eea3a87781d62b634e6','58615a403a87781fbdea1bf4'),(23,8,25,2,3,0,1,'2016-12-22 17:37:36',NULL,'58615a403a87781fbdea1bf4'),(24,9,26,2,3,0,0,'2016-12-22 19:03:08','585c22fb3a87781d62b634e8','58615a403a87781fbdea1bf4'),(25,7,27,2,3,0,1,'2016-12-26 20:07:57','58616b6d3a87781fbdea1bf7','58615a403a87781fbdea1bf4'),(26,15,28,2,3,26,1,'2016-12-27 17:12:31','5862a0fe3a877829cd437ed4','58615a403a87781fbdea1bf4'),(27,15,29,2,3,28,1,'2016-12-27 17:22:58','5862a3713a877829cd437ed5','58615a403a87781fbdea1bf4'),(28,15,30,2,3,0,1,'2017-01-03 11:23:46','586b883f3a877862cb6330d9','58615a403a87781fbdea1bf4'),(29,15,31,2,3,0,1,'2017-01-03 11:24:01','586b88653a877862cb6330da','58615a403a87781fbdea1bf4'),(30,15,32,2,3,0,1,'2017-01-03 11:24:31','586b888a3a877862cb6330db','58615a403a87781fbdea1bf4'),(31,15,33,2,3,0,1,'2017-01-03 11:24:51','586b88a73a877862cb6330dc','58615a403a87781fbdea1bf4'),(32,15,34,2,3,0,1,'2017-01-03 11:25:10','586b88c33a877862cb6330dd','58615a403a87781fbdea1bf4'),(33,15,35,2,3,0,1,'2017-01-03 11:25:31','586b88e13a877862cb6330de','58615a403a87781fbdea1bf4'),(34,15,36,2,3,0,1,'2017-01-03 11:26:47','586b89003a877862cb6330e0','58615a403a87781fbdea1bf4'),(35,15,37,2,3,0,1,'2017-01-03 11:26:59','586b89213a877862cb6330e1','58615a403a87781fbdea1bf4'),(36,15,38,2,3,0,1,'2017-01-03 11:27:16','586b89573a877862cb6330e2','58615a403a87781fbdea1bf4'),(37,16,39,3,1,0,1,'2017-01-09 16:37:53',NULL,NULL),(38,17,40,4,1,0,1,'2017-01-09 16:51:30',NULL,NULL),(39,18,41,5,1,0,1,'2017-01-09 16:56:57',NULL,NULL),(40,19,44,6,44,53,1,'2017-01-11 12:10:25','587617f03a87782347bda33d',''),(41,20,46,7,46,55,1,'2017-01-11 12:29:56','58761d483a87782347bda33f','58761d123a87782347bda33e'),(42,10,47,2,3,0,1,'2017-01-12 13:30:47',NULL,'58615a403a87781fbdea1bf4'),(43,10,48,2,3,0,1,'2017-01-12 16:18:13',NULL,'58615a403a87781fbdea1bf4'),(44,10,49,2,3,0,1,'2017-01-12 16:50:13',NULL,'58615a403a87781fbdea1bf4'),(45,10,50,2,3,0,1,'2017-01-12 16:56:31','5877a8373a87784e0880304d','58615a403a87781fbdea1bf4'),(46,10,51,2,3,0,1,'2017-01-13 23:57:17','587969513a877860e753eb55','58615a403a87781fbdea1bf4');
/*!40000 ALTER TABLE `Firm_Worker` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `Firm_Worker_Type`
--

DROP TABLE IF EXISTS `Firm_Worker_Type`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!40101 SET character_set_client = utf8 */;
CREATE TABLE `Firm_Worker_Type` (
  `Id` int(11) NOT NULL AUTO_INCREMENT,
  `TypeName` varchar(50) NOT NULL,
  `ProfileName` int(11) NOT NULL,
  `FirmId` int(11) NOT NULL,
  `UpdatedBy` int(11) NOT NULL,
  `Status` tinyint(4) NOT NULL,
  `LastUpdate` datetime NOT NULL,
  `ExternalFirmId` varchar(25) DEFAULT NULL,
  PRIMARY KEY (`Id`),
  KEY `FIRM_ID_PROFILE` (`FirmId`),
  CONSTRAINT `FIRM_ID_PROFILE` FOREIGN KEY (`FirmId`) REFERENCES `Firm_Data` (`Id`) ON DELETE NO ACTION ON UPDATE NO ACTION
) ENGINE=InnoDB AUTO_INCREMENT=22 DEFAULT CHARSET=latin1;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `Firm_Worker_Type`
--

LOCK TABLES `Firm_Worker_Type` WRITE;
/*!40000 ALTER TABLE `Firm_Worker_Type` DISABLE KEYS */;
INSERT INTO `Firm_Worker_Type` VALUES (1,'ADMINISTRADOR',0,1,1,1,'2016-12-05 12:28:47',''),(2,'Gerente',2,1,2,1,'2016-12-05 12:33:45',''),(3,'Vendedor',1,1,2,1,'2016-12-05 12:34:12',''),(4,'Supervisor de equipe',2,1,2,1,'2016-12-05 12:34:37',''),(5,'Diretor',2,1,2,1,'2016-12-05 12:34:59',''),(6,'ADMINISTRADOR',0,2,1,1,'2016-12-05 13:04:26',''),(7,'Supervisor',2,2,3,1,'2016-12-05 13:31:34',''),(8,'Gerente',2,2,3,1,'2016-12-05 13:32:09',''),(9,'Diretor',2,2,3,1,'2016-12-05 13:33:40',''),(10,'Vendedor Externo',1,2,3,1,'2016-12-27 14:17:59','58615a403a87781fbdea1bf4'),(11,'Atendente Ativo',1,2,3,1,'2016-12-05 13:34:05',''),(12,'fUNÇÃO tESTE',1,2,3,0,'2016-12-22 19:50:47',''),(13,'teste',2,2,3,0,'2016-12-22 19:53:01',''),(14,'teste',1,2,3,1,'2016-12-27 15:03:26','58615a403a87781fbdea1bf4'),(15,'Teste 2',2,2,3,1,'2016-12-27 14:58:38','58615a403a87781fbdea1bf4'),(16,'ADMINISTRADOR',0,3,1,1,'2017-01-09 16:37:53',NULL),(17,'ADMINISTRADOR',0,4,1,1,'2017-01-09 16:51:25',NULL),(18,'ADMINISTRADOR',0,5,1,1,'2017-01-09 16:56:57',NULL),(19,'ADMINISTRADOR',0,6,1,1,'2017-01-11 11:02:29',NULL),(20,'ADMINISTRADOR',0,7,1,1,'2017-01-11 11:55:40',NULL),(21,'f',0,2,3,1,'2017-01-17 10:27:01','58615a403a87781fbdea1bf4');
/*!40000 ALTER TABLE `Firm_Worker_Type` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `Firm_Worker_Type_Metric`
--

DROP TABLE IF EXISTS `Firm_Worker_Type_Metric`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!40101 SET character_set_client = utf8 */;
CREATE TABLE `Firm_Worker_Type_Metric` (
  `Id` int(11) NOT NULL AUTO_INCREMENT,
  `MetricId` int(11) DEFAULT NULL,
  `MetricExternalId` varchar(25) NOT NULL,
  `WorkerTypeId` int(11) NOT NULL,
  `UpdatedBy` int(11) NOT NULL,
  `LastUpdate` datetime NOT NULL,
  `Status` tinyint(4) NOT NULL,
  PRIMARY KEY (`Id`),
  KEY `METRIC_ID_WORKER_TYPE_METRIC` (`MetricId`),
  KEY `WORKER_TYPE_ID_WORKER_TYPE_METRIC` (`WorkerTypeId`),
  CONSTRAINT `METRIC_ID_WORKER_TYPE_METRIC` FOREIGN KEY (`MetricId`) REFERENCES `Firm_Metric` (`Id`) ON DELETE NO ACTION ON UPDATE NO ACTION,
  CONSTRAINT `WORKER_TYPE_ID_WORKER_TYPE_METRIC` FOREIGN KEY (`WorkerTypeId`) REFERENCES `Firm_Worker_Type` (`Id`) ON DELETE NO ACTION ON UPDATE NO ACTION
) ENGINE=InnoDB AUTO_INCREMENT=56 DEFAULT CHARSET=latin1;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `Firm_Worker_Type_Metric`
--

LOCK TABLES `Firm_Worker_Type_Metric` WRITE;
/*!40000 ALTER TABLE `Firm_Worker_Type_Metric` DISABLE KEYS */;
INSERT INTO `Firm_Worker_Type_Metric` VALUES (1,9,'',7,3,'2016-12-05 18:08:47',1),(2,9,'',8,3,'2016-12-05 18:08:47',1),(3,9,'',10,3,'2016-12-05 18:12:35',1),(4,1,'',2,2,'2016-12-06 13:25:01',1),(5,1,'',3,2,'2016-12-06 13:25:03',1),(6,1,'',4,2,'2016-12-06 13:25:04',1),(7,1,'',5,2,'2016-12-06 13:25:05',1),(8,5,'',2,2,'2016-12-06 13:25:22',1),(9,5,'',3,2,'2016-12-06 13:25:24',1),(10,5,'',4,2,'2016-12-06 13:25:25',1),(11,5,'',5,2,'2016-12-06 13:25:25',1),(12,6,'',2,2,'2016-12-06 13:25:33',1),(13,6,'',3,2,'2016-12-06 13:25:34',1),(14,6,'',4,2,'2016-12-06 13:25:35',1),(15,6,'',5,2,'2016-12-06 13:25:36',1),(16,7,'',2,2,'2016-12-06 13:25:41',1),(17,7,'',3,2,'2016-12-06 13:25:42',1),(18,7,'',4,2,'2016-12-06 13:25:43',1),(19,7,'',5,2,'2016-12-06 13:25:43',1),(20,2,'',2,2,'2016-12-06 13:25:48',1),(21,2,'',3,2,'2016-12-06 13:25:50',1),(22,2,'',4,2,'2016-12-06 13:25:51',1),(23,2,'',5,2,'2016-12-06 13:25:52',1),(24,4,'',2,2,'2016-12-06 13:25:58',1),(25,4,'',3,2,'2016-12-06 13:25:59',1),(26,4,'',4,2,'2016-12-06 13:25:59',1),(27,4,'',5,2,'2016-12-06 13:26:00',1),(28,3,'',2,2,'2016-12-06 13:26:01',1),(29,3,'',3,2,'2016-12-06 13:26:03',1),(30,3,'',4,2,'2016-12-06 13:26:04',1),(31,3,'',5,2,'2016-12-06 13:26:04',1),(32,8,'',2,2,'2016-12-06 13:26:18',1),(33,8,'',3,2,'2016-12-06 13:26:19',1),(34,8,'',4,2,'2016-12-06 13:26:20',1),(35,8,'',5,2,'2016-12-06 13:26:20',1),(36,9,'',8,3,'2016-12-22 18:41:45',1),(42,9,'58615e823a87781fbdea1bf6',8,3,'2016-12-27 12:59:43',0),(43,9,'58615e823a87781fbdea1bf6',7,3,'2016-12-27 12:55:44',0),(45,9,'58615caa3a87781fbdea1bf5',11,3,'2016-12-27 15:04:03',0),(46,9,'58615caa3a87781fbdea1bf5',10,3,'2016-12-27 10:47:09',1),(47,9,'58615caa3a87781fbdea1bf5',14,3,'2016-12-27 12:47:51',1),(48,9,'5861435b3a87781e730bb161',14,3,'2016-12-27 13:12:32',0),(51,9,'5861435b3a87781e730bb161',15,3,'2016-12-27 13:18:15',1),(52,9,'5861435b3a87781e730bb161',14,3,'2016-12-27 13:18:04',1),(53,9,'586283ef3a87782919f3a29f',10,3,'2017-01-02 16:06:36',0),(54,9,'586283ef3a87782919f3a29f',15,3,'2017-01-05 16:59:41',1),(55,9,'5873cb8d3a87781f8fdf4d5e',15,3,'2017-01-09 18:28:47',1);
/*!40000 ALTER TABLE `Firm_Worker_Type_Metric` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `Media_Image`
--

DROP TABLE IF EXISTS `Media_Image`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!40101 SET character_set_client = utf8 */;
CREATE TABLE `Media_Image` (
  `Id` int(11) NOT NULL AUTO_INCREMENT,
  `UpdatedBy` int(11) NOT NULL,
  `Status` tinyint(4) NOT NULL,
  `LastUpdate` datetime NOT NULL,
  PRIMARY KEY (`Id`)
) ENGINE=InnoDB AUTO_INCREMENT=57 DEFAULT CHARSET=latin1;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `Media_Image`
--

LOCK TABLES `Media_Image` WRITE;
/*!40000 ALTER TABLE `Media_Image` DISABLE KEYS */;
INSERT INTO `Media_Image` VALUES (1,1,1,'2016-12-05 12:28:45'),(2,1,1,'2016-12-05 13:04:22'),(4,3,1,'2016-12-05 13:38:07'),(5,3,1,'2016-12-05 13:38:39'),(6,3,1,'2016-12-05 13:48:54'),(7,3,1,'2016-12-05 17:33:39'),(8,3,1,'2016-12-05 17:34:07'),(9,3,1,'2016-12-05 17:37:05'),(10,2,1,'2016-12-06 10:39:56'),(11,2,1,'2016-12-06 10:40:22'),(12,2,1,'2016-12-06 10:41:03'),(13,2,1,'2016-12-06 10:41:38'),(14,2,1,'2016-12-06 11:23:14'),(15,2,1,'2016-12-06 11:24:32'),(16,2,1,'2016-12-06 11:25:17'),(17,2,1,'2016-12-06 11:25:57'),(18,2,1,'2016-12-06 11:26:40'),(19,2,1,'2016-12-06 13:27:05'),(20,2,1,'2016-12-06 13:27:26'),(21,2,1,'2016-12-06 13:27:41'),(22,2,1,'2016-12-06 13:27:59'),(23,2,1,'2016-12-06 16:36:32'),(24,3,1,'2016-12-19 16:16:45'),(25,3,1,'2016-12-19 16:17:09'),(26,3,1,'2016-12-27 17:12:27'),(28,3,1,'2016-12-27 17:22:45'),(30,3,1,'2016-12-29 18:08:06'),(32,3,1,'2016-12-29 18:35:40'),(33,3,1,'2016-12-30 11:59:16'),(34,3,1,'2016-12-30 12:23:10'),(35,3,1,'2017-01-04 10:43:40'),(36,3,1,'2017-01-04 11:58:21'),(37,3,1,'2017-01-04 12:04:41'),(38,3,1,'2017-01-04 12:06:02'),(39,3,1,'2017-01-04 12:07:03'),(40,3,1,'2017-01-04 12:08:55'),(43,3,1,'2017-01-04 13:55:34'),(44,3,1,'2017-01-04 15:40:10'),(47,1,1,'2017-01-11 11:08:13'),(53,44,1,'2017-01-11 12:10:25'),(54,1,1,'2017-01-11 12:26:25'),(55,46,1,'2017-01-11 12:29:56'),(56,3,1,'2017-01-11 18:31:01');
/*!40000 ALTER TABLE `Media_Image` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `Public_Help`
--

DROP TABLE IF EXISTS `Public_Help`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!40101 SET character_set_client = utf8 */;
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
) ENGINE=InnoDB DEFAULT CHARSET=latin1;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `Public_Help`
--

LOCK TABLES `Public_Help` WRITE;
/*!40000 ALTER TABLE `Public_Help` DISABLE KEYS */;
/*!40000 ALTER TABLE `Public_Help` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `Public_TopicHelp`
--

DROP TABLE IF EXISTS `Public_TopicHelp`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!40101 SET character_set_client = utf8 */;
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
) ENGINE=InnoDB AUTO_INCREMENT=5 DEFAULT CHARSET=latin1;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `Public_TopicHelp`
--

LOCK TABLES `Public_TopicHelp` WRITE;
/*!40000 ALTER TABLE `Public_TopicHelp` DISABLE KEYS */;
INSERT INTO `Public_TopicHelp` VALUES (1,1,'Ajuda',2,'2016-12-05 18:44:17',3),(2,0,'Ajuda',2,'2016-12-05 18:44:35',3),(3,0,'Ajuda',2,'2016-12-05 18:44:30',3),(4,1,'Teste',2,'2017-01-08 15:44:19',3);
/*!40000 ALTER TABLE `Public_TopicHelp` ENABLE KEYS */;
UNLOCK TABLES;
/*!40103 SET TIME_ZONE=@OLD_TIME_ZONE */;

/*!40101 SET SQL_MODE=@OLD_SQL_MODE */;
/*!40014 SET FOREIGN_KEY_CHECKS=@OLD_FOREIGN_KEY_CHECKS */;
/*!40014 SET UNIQUE_CHECKS=@OLD_UNIQUE_CHECKS */;
/*!40101 SET CHARACTER_SET_CLIENT=@OLD_CHARACTER_SET_CLIENT */;
/*!40101 SET CHARACTER_SET_RESULTS=@OLD_CHARACTER_SET_RESULTS */;
/*!40101 SET COLLATION_CONNECTION=@OLD_COLLATION_CONNECTION */;
/*!40111 SET SQL_NOTES=@OLD_SQL_NOTES */;

-- Dump completed on 2017-01-17 10:02:38
