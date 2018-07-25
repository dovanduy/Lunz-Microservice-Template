
SET FOREIGN_KEY_CHECKS=0;

-- ----------------------------
-- Table structure for hearfroms
-- ----------------------------
CREATE TABLE `hearfroms` (
  `Id` char(36) NOT NULL,
  `Name` varchar(255) NOT NULL,
  PRIMARY KEY (`Id`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8;