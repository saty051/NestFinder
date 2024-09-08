using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace WebAPI.Migrations
{
    /// <inheritdoc />
    public partial class SeedDemoData : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql(@"
               DECLARE @UserID as INT
                --------------------------
                -- Create User
                --------------------------
                IF NOT EXISTS (SELECT Id FROM Users WHERE Username='Demo')
                INSERT INTO Users(Username,Password, PasswordKey,LastUpdatedOn,LatestUpdatedBy)
                SELECT 'Demo',
                0x4D5544D09B8319B423F6D4E054360D5289B57A98781A66B276E00C57919FDCD599BF45623D48CC81F535748F560AF0F70C8C7F3B4C3DB672562B5DD0E5E7C297,
                0x44A0BD5BFD689DF399346200A1117C33BEDF5869C17A7CB3DC6D8598A93845DB333B379AA90931D8D4E5F2CC7B1A4A96A7DB71B186DBCDCDC53B0A95440E4EDD7473668627970FBD9BB0BA17530CCAB2D9446A1902BD6AC12FE691FE09DD78A43398B89111056145843060026A414FFA8C5E75B474E187AD753D2872038D9FDD,
                GETDATE(),
                0

                SET @UserID = (SELECT Id FROM Users WHERE Username='Demo')

                --------------------------
                -- Seed Property Types
                --------------------------
                IF NOT EXISTS (SELECT Name FROM PropertyTypes WHERE Name='House')
                INSERT INTO PropertyTypes(Name,LastUpdatedOn,LatestUpdatedBy)
                SELECT 'House', GETDATE(), @UserID

                IF NOT EXISTS (SELECT Name FROM PropertyTypes WHERE Name='Apartment')
                INSERT INTO PropertyTypes(Name,LastUpdatedOn,LatestUpdatedBy)
                SELECT 'Apartment', GETDATE(), @UserID
	
                IF NOT EXISTS (SELECT Name FROM PropertyTypes WHERE Name='Duplex')
                INSERT INTO PropertyTypes(Name,LastUpdatedOn,LatestUpdatedBy)
                SELECT 'Duplex', GETDATE(), @UserID

                --------------------------
                -- Seed Furnishing Types
                --------------------------
                IF NOT EXISTS (SELECT Name FROM FurnishingTypes WHERE Name='Fully')
                INSERT INTO FurnishingTypes(Name, LastUpdatedOn, LatestUpdatedBy)
                SELECT 'Fully', GETDATE(), @UserID
	
                IF NOT EXISTS (SELECT Name FROM FurnishingTypes WHERE Name='Semi')
                INSERT INTO FurnishingTypes(Name, LastUpdatedOn, LatestUpdatedBy)
                SELECT 'Semi', GETDATE(), @UserID
	
                IF NOT EXISTS (SELECT Name FROM FurnishingTypes WHERE Name='Unfurnished')
                INSERT INTO FurnishingTypes(Name, LastUpdatedOn, LatestUpdatedBy)
                SELECT 'Unfurnished', GETDATE(), @UserID

                --------------------------
                -- Seed Cities
                --------------------------
                IF NOT EXISTS (SELECT TOP 1 Id FROM Cities)
                INSERT INTO Cities(Name,LatestUpdatedBy,LastUpdatedOn,Country)
                SELECT 'New York', @UserID, GETDATE(), 'USA'
                UNION
                SELECT 'Houston', @UserID, GETDATE(), 'USA'
                UNION
                SELECT 'Los Angeles', @UserID, GETDATE(), 'USA'
                UNION
                SELECT 'New Delhi', @UserID, GETDATE(), 'India'
                UNION
                SELECT 'Bangalore', @UserID, GETDATE(), 'India'

                --------------------------
                -- Seed Properties
                --------------------------
                -- Seed property for sell
                IF NOT EXISTS (SELECT TOP 1 Name FROM Properties WHERE Name='White House Demo')
                INSERT INTO Properties(SellRent,Name,PropertyTypeId,BHK,FurnishingTypeId,Price,BuiltArea,CarpetArea,Address,
                Address2,CityId,FloorNo,TotalFloors,ReadyToMove,MainEntrance,Security,Gated,Maintenance,EstPossessionOn,Age,Description,PostedOn,PostedBy,LastUpdatedOn,LatestUpdatedBy)
                SELECT 
                1, -- Sell Rent
                'White House Demo', -- Name
                (SELECT Id FROM PropertyTypes WHERE Name='Apartment'), -- Property Type ID
                2, -- BHK
                (SELECT Id FROM FurnishingTypes WHERE Name='Fully'), -- Furnishing Type ID
                1800, -- Price
                1400, -- Built Area
                900, -- Carpet Area
                '6 Street', -- Address
                'Golf Course Road', -- Address2
                (SELECT TOP 1 Id FROM Cities), -- City ID
                3, -- Floor No
                3, -- Total Floors
                1, -- Ready to Move
                'East', -- Main Entrance
                0, -- Security
                1, -- Gated
                300, -- Maintenance
                '2019-01-01', -- Establishment or Possession on
                0, -- Age
                'Well Maintained builder floor available for rent at prime location. # property features- - 5 mins away from metro station - Gated community - 24*7 security. # property includes- - Big rooms (Cross ventilation & proper sunlight) - 
                Drawing and dining area - Washrooms - Balcony - Modular kitchen - Near gym, market, temple and park - Easy commuting to major destination. Feel free to call With Query.', -- Description
                GETDATE(), -- Posted on
                @UserID, -- Posted by
                GETDATE(), -- Last Updated on
                @UserID -- Latest Updated by

                ---------------------------
                -- Seed property for rent
                ---------------------------
                IF NOT EXISTS (SELECT TOP 1 Name FROM Properties WHERE Name='Birla House Demo')
                INSERT INTO Properties(SellRent,Name,PropertyTypeId,BHK,FurnishingTypeId,Price,BuiltArea,CarpetArea,Address,
                Address2,CityId,FloorNo,TotalFloors,ReadyToMove,MainEntrance,Security,Gated,Maintenance,EstPossessionOn,Age,Description,PostedOn,PostedBy,LastUpdatedOn,LatestUpdatedBy)
                SELECT 
                2, -- Sell Rent
                'Birla House Demo', -- Name
                (SELECT Id FROM PropertyTypes WHERE Name='Apartment'), -- Property Type ID
                2, -- BHK
                (SELECT Id FROM FurnishingTypes WHERE Name='Fully'), -- Furnishing Type ID
                1800, -- Price
                1400, -- Built Area
                900, -- Carpet Area
                '6 Street', -- Address
                'Golf Course Road', -- Address2
                (SELECT TOP 1 Id FROM Cities), -- City ID
                3, -- Floor No
                3, -- Total Floors
                1, -- Ready to Move
                'East', -- Main Entrance
                0, -- Security
                1, -- Gated
                300, -- Maintenance
                '2019-01-01', -- Establishment or Possession on
                0, -- Age
                'Well Maintained builder floor available for rent at prime location. # property features- - 5 mins away from metro station - Gated community - 24*7 security. # property includes- - Big rooms (Cross ventilation & proper sunlight) - 
                Drawing and dining area - Washrooms - Balcony - Modular kitchen - Near gym, market, temple and park - Easy commuting to major destination. Feel free to call With Query.', -- Description
                GETDATE(), -- Posted on
                @UserID, -- Posted by
                GETDATE(), -- Last Updated on
                @UserID -- Latest Updated by
            ");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql(@"
             --------------------
             -- Seeding Down
             --------------------
             DECLARE @UserID as int
             SET @UserID = (SELECT id FROM Users WHERE Username='Demo')

             DELETE FROM Users WHERE Username='Demo'
             DELETE FROM PropertyTypes WHERE LatestUpdatedBy=@UserID
             DELETE FROM FurnishingTypes WHERE LatestUpdatedBy=@UserID
             DELETE FROM Cities WHERE LatestUpdatedBy=@UserID
             DELETE FROM Properties WHERE PostedBy=@UserID
            ");
        }
    }
}
