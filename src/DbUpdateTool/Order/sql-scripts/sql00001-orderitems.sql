SET FOREIGN_KEY_CHECKS=0;

-- ----------------------------
-- Table structure for orderitems
-- ----------------------------

CREATE TABLE `orderitems` (
  `Id` char(36) NOT NULL,
  `OrderId` char(36) NOT NULL,
  `ProductName` varchar(255) NOT NULL,
  `Amount` int(11) NOT NULL,
  `Price` decimal(10,2) NOT NULL,
  `Total` decimal(10,2) NOT NULL,
  `CreatedById` char(36) DEFAULT NULL,
  `CreatedAt` datetime DEFAULT NULL,
  `UpdatedById` char(36) DEFAULT NULL,
  `UpdatedAt` datetime DEFAULT NULL,
  `Deleted` bit(1) NOT NULL,
  `DeletedById` char(36) DEFAULT NULL,
  `DeletedAt` datetime DEFAULT NULL,
  PRIMARY KEY (`Id`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8;
