using Autofac.Extras.Moq;
using FanDuel.DepthChart.Application.Interfaces.Repositories;
using FanDuel.DepthChart.Application.NFL;
using FanDuel.DepthChart.Contracts;
using FanDuel.DepthChart.Domain.Entities;
using FanDuel.DepthChart.Domain.Types;
using Moq;

namespace FanDuel.DepthChart.Tests
{
    public class DepthChartTest
    {
        [Fact]
        public void AddPlayerToDepthChart_When_EmptyPosition()
        {
            //Arrange
            var tomBrady = new PlayerDto { Name = "Tom Brady", Number = 12 };
            var blaineGabbert = new PlayerDto { Name = "Blaine Gabbert", Number = 11 };

            var teamDepthChart = new TeamDepthChart(1);

            using var mock = AutoMock.GetStrict();
            mock.Mock<IRepository>().Setup(x => x.GetTeamDepthChart(It.IsInRange(1, 32, Moq.Range.Inclusive))).Returns(teamDepthChart);
            var manager = mock.Create<NflDepthChartManager>();

            //Act
            manager.AddPlayerToDepthChart(NflPositionTypes.LG.ToString(), tomBrady, 0);
            manager.AddPlayerToDepthChart(NflPositionTypes.LG.ToString(), blaineGabbert, 1);
            manager.AddPlayerToDepthChart(NflPositionTypes.C.ToString(), blaineGabbert, 1);

            //Assert
            Assert.True(teamDepthChart.Entries.Count == 2);
            Assert.True(teamDepthChart.Entries[NflPositionTypes.LG.ToString()].Count == 2);
            Assert.True(teamDepthChart.Entries[NflPositionTypes.C.ToString()].Count == 1);
        }

        [Fact]
        public void AddPlayerToDepthChartThrows_When_AddingDuplicatePlayersToSamePosition()
        {
            //Arrange
            var tomBrady = new PlayerDto { Name = "Tom Brady", Number = 12 };
            var blaineGabbert = new PlayerDto { Name = "Blaine Gabbert", Number = 11 };

            var teamDepthChart = new TeamDepthChart(1);

            using var mock = AutoMock.GetStrict();
            mock.Mock<IRepository>().Setup(x => x.GetTeamDepthChart(It.IsInRange(1, 32, Moq.Range.Inclusive))).Returns(teamDepthChart);
            var manager = mock.Create<NflDepthChartManager>();

            //Act
            manager.AddPlayerToDepthChart(NflPositionTypes.LG.ToString(), tomBrady, 0);
            manager.AddPlayerToDepthChart(NflPositionTypes.LG.ToString(), blaineGabbert, 1);
            var exception = Record.Exception(() => manager.AddPlayerToDepthChart(NflPositionTypes.LG.ToString(), blaineGabbert, 1));

            //Assert
            Assert.NotNull(exception);
        }

        [Fact]
        public void AddPlayerToDepthChartThrows_When_AddingInvalidPosition()
        {
            //Arrange
            var tomBrady = new PlayerDto { Name = "Tom Brady", Number = 12 };
            var blaineGabbert = new PlayerDto { Name = "Blaine Gabbert", Number = 11 };

            var teamDepthChart = new TeamDepthChart(1);

            using var mock = AutoMock.GetStrict();
            mock.Mock<IRepository>().Setup(x => x.GetTeamDepthChart(It.IsInRange(1, 32, Moq.Range.Inclusive))).Returns(teamDepthChart);
            var manager = mock.Create<NflDepthChartManager>();

            //Act
            manager.AddPlayerToDepthChart(NflPositionTypes.LG.ToString(), tomBrady, 0);
            manager.AddPlayerToDepthChart(NflPositionTypes.LG.ToString(), blaineGabbert, 1);
            var exception = Record.Exception(() => manager.AddPlayerToDepthChart("BB", blaineGabbert, 1));

            //Assert
            Assert.NotNull(exception);
        }

        [Fact]
        public void AddPlayerToEndOfDepthChart_When_NoPositionProvided()
        {
            //Arrange
            var tomBrady = new PlayerDto { Name = "Tom Brady", Number = 12 };
            var blaineGabbert = new PlayerDto { Name = "Blaine Gabbert", Number = 11 };

            var teamDepthChart = new TeamDepthChart(1);

            using var mock = AutoMock.GetStrict();
            mock.Mock<IRepository>().Setup(x => x.GetTeamDepthChart(It.IsInRange(1, 32, Moq.Range.Inclusive))).Returns(teamDepthChart);
            var manager = mock.Create<NflDepthChartManager>();

            //Act
            manager.AddPlayerToDepthChart(NflPositionTypes.LG.ToString(), tomBrady);
            manager.AddPlayerToDepthChart(NflPositionTypes.LG.ToString(), blaineGabbert);

            //Assert
            Assert.True(teamDepthChart.Entries[NflPositionTypes.LG.ToString()][0].Player.Number == tomBrady.Number);
            Assert.True(teamDepthChart.Entries[NflPositionTypes.LG.ToString()][1].Player.Number == blaineGabbert.Number);
        }

        [Fact]
        public void MoveDownOtherPlayersOnePosition_When_NewPlayerIsAdded()
        {
            //Arrange
            var tomBrady = new PlayerDto { Name = "Tom Brady", Number = 12 };
            var blaineGabbert = new PlayerDto { Name = "Blaine Gabbert", Number = 11 };
            var jaelonDarden = new PlayerDto { Name = "Jaelon Darden", Number = 1 };
            var scottMiller = new PlayerDto { Name = "Scott Miller", Number = 10 };
            var mikeEvans = new PlayerDto { Name = "Mike Evans", Number = 13 };

            var teamDepthChart = new TeamDepthChart(1);

            using var mock = AutoMock.GetStrict();
            mock.Mock<IRepository>().Setup(x => x.GetTeamDepthChart(It.IsInRange(1, 32, Moq.Range.Inclusive))).Returns(teamDepthChart);
            var manager = mock.Create<NflDepthChartManager>();

            //Act
            manager.AddPlayerToDepthChart(NflPositionTypes.LG.ToString(), tomBrady, 0);
            manager.AddPlayerToDepthChart(NflPositionTypes.LG.ToString(), blaineGabbert, 1);
            manager.AddPlayerToDepthChart(NflPositionTypes.LG.ToString(), jaelonDarden, 1);
            manager.AddPlayerToDepthChart(NflPositionTypes.LG.ToString(), scottMiller, 1);
            manager.AddPlayerToDepthChart(NflPositionTypes.LG.ToString(), mikeEvans, 2);

            //Assert
            Assert.True(teamDepthChart.Entries[NflPositionTypes.LG.ToString()][0].Player.Number == tomBrady.Number);
            Assert.True(teamDepthChart.Entries[NflPositionTypes.LG.ToString()][1].Player.Number == scottMiller.Number);
            Assert.True(teamDepthChart.Entries[NflPositionTypes.LG.ToString()][1].Rank == 1);
            Assert.True(teamDepthChart.Entries[NflPositionTypes.LG.ToString()][2].Player.Number == mikeEvans.Number);
            Assert.True(teamDepthChart.Entries[NflPositionTypes.LG.ToString()][2].Rank == 2);
            Assert.True(teamDepthChart.Entries[NflPositionTypes.LG.ToString()][3].Player.Number == jaelonDarden.Number);
            Assert.True(teamDepthChart.Entries[NflPositionTypes.LG.ToString()][3].Rank == 3);
            Assert.True(teamDepthChart.Entries[NflPositionTypes.LG.ToString()][4].Player.Number == blaineGabbert.Number);
            Assert.True(teamDepthChart.Entries[NflPositionTypes.LG.ToString()][4].Rank == 4);
        }

        [Fact]
        public void RemovePlayerFromDepthChart_When_PositionExists()
        {
            //Arrange
            var tomBrady = new PlayerDto { Name = "Tom Brady", Number = 12 };
            var blaineGabbert = new PlayerDto { Name = "Blaine Gabbert", Number = 11 };
            var jaelonDarden = new PlayerDto { Name = "Jaelon Darden", Number = 1 };
            var scottMiller = new PlayerDto { Name = "Scott Miller", Number = 10 };
            var mikeEvans = new PlayerDto { Name = "Mike Evans", Number = 13 };

            var teamDepthChart = new TeamDepthChart(1);

            using var mock = AutoMock.GetStrict();
            mock.Mock<IRepository>().Setup(x => x.GetTeamDepthChart(It.IsInRange(1, 32, Moq.Range.Inclusive))).Returns(teamDepthChart);
            var manager = mock.Create<NflDepthChartManager>();

            //Act
            manager.AddPlayerToDepthChart(NflPositionTypes.LG.ToString(), blaineGabbert, 1);
            manager.AddPlayerToDepthChart(NflPositionTypes.LG.ToString(), jaelonDarden, 1);
            manager.AddPlayerToDepthChart(NflPositionTypes.LG.ToString(), scottMiller, 1);
            manager.AddPlayerToDepthChart(NflPositionTypes.LG.ToString(), mikeEvans, 2);
            manager.AddPlayerToDepthChart(NflPositionTypes.QB.ToString(), mikeEvans, 2);
            manager.AddPlayerToDepthChart(NflPositionTypes.RWR.ToString(), mikeEvans, 2);

            manager.RemovePlayerFromDepthChart(NflPositionTypes.RWR.ToString(), mikeEvans);
            manager.RemovePlayerFromDepthChart(NflPositionTypes.LG.ToString(), mikeEvans);

            //Assert
            Assert.DoesNotContain(teamDepthChart.Entries[NflPositionTypes.RWR.ToString()], x => x.Player.Number == mikeEvans.Number);
            Assert.DoesNotContain(teamDepthChart.Entries[NflPositionTypes.LG.ToString()], x => x.Player.Number == mikeEvans.Number);
            Assert.Contains(teamDepthChart.Entries[NflPositionTypes.QB.ToString()], x => x.Player.Number == mikeEvans.Number);
        }

        [Fact]
        public void MoveUpOtherPlayersOnePosition_When_APlayerIsRemoved()
        {
            //Arrange
            var tomBrady = new PlayerDto { Name = "Tom Brady", Number = 12 };
            var blaineGabbert = new PlayerDto { Name = "Blaine Gabbert", Number = 11 };
            var jaelonDarden = new PlayerDto { Name = "Jaelon Darden", Number = 1 };
            var scottMiller = new PlayerDto { Name = "Scott Miller", Number = 10 };
            var mikeEvans = new PlayerDto { Name = "Mike Evans", Number = 13 };

            var teamDepthChart = new TeamDepthChart(1);

            using var mock = AutoMock.GetStrict();
            mock.Mock<IRepository>().Setup(x => x.GetTeamDepthChart(It.IsInRange(1, 32, Moq.Range.Inclusive))).Returns(teamDepthChart);
            var manager = mock.Create<NflDepthChartManager>();

            //Act
            manager.AddPlayerToDepthChart(NflPositionTypes.LG.ToString(), blaineGabbert, 1);
            manager.AddPlayerToDepthChart(NflPositionTypes.LG.ToString(), jaelonDarden, 1);
            manager.AddPlayerToDepthChart(NflPositionTypes.LG.ToString(), scottMiller, 1);
            manager.AddPlayerToDepthChart(NflPositionTypes.LG.ToString(), mikeEvans, 2);

            manager.RemovePlayerFromDepthChart(NflPositionTypes.LG.ToString(), jaelonDarden);

            //Assert
            Assert.True(teamDepthChart.Entries[NflPositionTypes.LG.ToString()][0].Player.Number == blaineGabbert.Number);
            Assert.True(teamDepthChart.Entries[NflPositionTypes.LG.ToString()][0].Rank == 0);
            Assert.True(teamDepthChart.Entries[NflPositionTypes.LG.ToString()][1].Player.Number == scottMiller.Number);
            Assert.True(teamDepthChart.Entries[NflPositionTypes.LG.ToString()][1].Rank == 1);
            Assert.True(teamDepthChart.Entries[NflPositionTypes.LG.ToString()][2].Player.Number == mikeEvans.Number);
            Assert.True(teamDepthChart.Entries[NflPositionTypes.LG.ToString()][2].Rank == 2);
        }

        [Fact]
        public void ReturnRemovedPlayer_When_PlayerExistInGivenPosition()
        {
            //Arrange
            var tomBrady = new PlayerDto { Name = "Tom Brady", Number = 12 };
            var blaineGabbert = new PlayerDto { Name = "Blaine Gabbert", Number = 11 };
            var jaelonDarden = new PlayerDto { Name = "Jaelon Darden", Number = 1 };
            var scottMiller = new PlayerDto { Name = "Scott Miller", Number = 10 };
            var mikeEvans = new PlayerDto { Name = "Mike Evans", Number = 13 };

            var teamDepthChart = new TeamDepthChart(1);

            using var mock = AutoMock.GetStrict();
            mock.Mock<IRepository>().Setup(x => x.GetTeamDepthChart(It.IsInRange(1, 32, Moq.Range.Inclusive))).Returns(teamDepthChart);
            var manager = mock.Create<NflDepthChartManager>();

            //Act
            manager.AddPlayerToDepthChart(NflPositionTypes.LG.ToString(), blaineGabbert, 1);
            manager.AddPlayerToDepthChart(NflPositionTypes.LG.ToString(), jaelonDarden, 1);
            manager.AddPlayerToDepthChart(NflPositionTypes.LG.ToString(), scottMiller, 1);
            manager.AddPlayerToDepthChart(NflPositionTypes.LG.ToString(), mikeEvans, 2);

            var removedPlayers = manager.RemovePlayerFromDepthChart(NflPositionTypes.LG.ToString(), jaelonDarden);

            //Assert
            Assert.Contains(removedPlayers, x => x.Number == jaelonDarden.Number);
        }

        [Fact]
        public void ReturnEmptyList_When_PlayerDoesNotExistInGivenPosition()
        {
            //Arrange
            var blaineGabbert = new PlayerDto { Name = "Blaine Gabbert", Number = 11 };
            var jaelonDarden = new PlayerDto { Name = "Jaelon Darden", Number = 1 };
            var scottMiller = new PlayerDto { Name = "Scott Miller", Number = 10 };
            var mikeEvans = new PlayerDto { Name = "Mike Evans", Number = 13 };

            var teamDepthChart = new TeamDepthChart(1);

            using var mock = AutoMock.GetStrict();
            mock.Mock<IRepository>().Setup(x => x.GetTeamDepthChart(It.IsInRange(1, 32, Moq.Range.Inclusive))).Returns(teamDepthChart);
            var manager = mock.Create<NflDepthChartManager>();

            //Act
            manager.AddPlayerToDepthChart(NflPositionTypes.LG.ToString(), blaineGabbert, 1);
            manager.AddPlayerToDepthChart(NflPositionTypes.LG.ToString(), scottMiller, 1);
            manager.AddPlayerToDepthChart(NflPositionTypes.LG.ToString(), mikeEvans, 2);

            var removedPlayers = manager.RemovePlayerFromDepthChart(NflPositionTypes.LG.ToString(), jaelonDarden);

            //Assert
            Assert.Equal(removedPlayers, new List<PlayerDto>());
        }

        [Fact]
        public void Throws_When_InvalidPosition()
        {
            //Arrange
            var tomBrady = new PlayerDto { Name = "Tom Brady", Number = 12 };

            var teamDepthChart = new TeamDepthChart(1);

            using var mock = AutoMock.GetStrict();
            mock.Mock<IRepository>().Setup(x => x.GetTeamDepthChart(It.IsInRange(1, 32, Moq.Range.Inclusive))).Returns(teamDepthChart);
            var manager = mock.Create<NflDepthChartManager>();

            //Act
            //Assert
            var exception = Record.Exception(() => manager.RemovePlayerFromDepthChart(NflPositionTypes.RWR.ToString(), tomBrady));
            Assert.NotNull(exception);
        }

        [Fact]
        public void GetBackups_When_BackupsExist()
        {
            //Arrange
            var blaineGabbert = new PlayerDto { Name = "Blaine Gabbert", Number = 11 };
            var scottMiller = new PlayerDto { Name = "Scott Miller", Number = 10 };
            var mikeEvans = new PlayerDto { Name = "Mike Evans", Number = 13 };

            var teamDepthChart = new TeamDepthChart(1);

            using var mock = AutoMock.GetStrict();
            mock.Mock<IRepository>().Setup(x => x.GetTeamDepthChart(It.IsInRange(1, 32, Moq.Range.Inclusive))).Returns(teamDepthChart);
            var manager = mock.Create<NflDepthChartManager>();

            //Act
            manager.AddPlayerToDepthChart(NflPositionTypes.LG.ToString(), blaineGabbert, 0);
            manager.AddPlayerToDepthChart(NflPositionTypes.LG.ToString(), scottMiller, 1);
            manager.AddPlayerToDepthChart(NflPositionTypes.LG.ToString(), mikeEvans, 2);

            var backups = manager.GetBackups(NflPositionTypes.LG.ToString(), blaineGabbert);

            //Assert
            Assert.Contains(backups, x => x.Number == scottMiller.Number);
            Assert.Contains(backups, x => x.Number == mikeEvans.Number);
        }

        [Fact]
        public void GetEmptyList_When_GivenPlayerDoesNotExist()
        {
            //Arrange
            var tomBrady = new PlayerDto { Name = "Tom Brady", Number = 12 };
            var blaineGabbert = new PlayerDto { Name = "Blaine Gabbert", Number = 11 };
            var scottMiller = new PlayerDto { Name = "Scott Miller", Number = 10 };
            var mikeEvans = new PlayerDto { Name = "Mike Evans", Number = 13 };

            var teamDepthChart = new TeamDepthChart(1);

            using var mock = AutoMock.GetStrict();
            mock.Mock<IRepository>().Setup(x => x.GetTeamDepthChart(It.IsInRange(1, 32, Moq.Range.Inclusive))).Returns(teamDepthChart);
            var manager = mock.Create<NflDepthChartManager>();

            //Act
            manager.AddPlayerToDepthChart(NflPositionTypes.LG.ToString(), blaineGabbert, 0);
            manager.AddPlayerToDepthChart(NflPositionTypes.LG.ToString(), scottMiller, 1);
            manager.AddPlayerToDepthChart(NflPositionTypes.LG.ToString(), mikeEvans, 2);

            var backups = manager.GetBackups(NflPositionTypes.LG.ToString(), tomBrady);

            //Assert
            Assert.Equal(backups, new List<PlayerDto>());
        }

        [Fact]
        public void GetEmptyList_When_NoBackups()
        {
            //Arrange
            var blaineGabbert = new PlayerDto { Name = "Blaine Gabbert", Number = 11 };
            var scottMiller = new PlayerDto { Name = "Scott Miller", Number = 10 };
            var mikeEvans = new PlayerDto { Name = "Mike Evans", Number = 13 };

            var teamDepthChart = new TeamDepthChart(1);

            using var mock = AutoMock.GetStrict();
            mock.Mock<IRepository>().Setup(x => x.GetTeamDepthChart(It.IsInRange(1, 32, Moq.Range.Inclusive))).Returns(teamDepthChart);
            var manager = mock.Create<NflDepthChartManager>();

            //Act
            manager.AddPlayerToDepthChart(NflPositionTypes.LG.ToString(), blaineGabbert, 0);
            manager.AddPlayerToDepthChart(NflPositionTypes.LG.ToString(), scottMiller, 1);
            manager.AddPlayerToDepthChart(NflPositionTypes.LG.ToString(), mikeEvans, 2);

            var backups = manager.GetBackups(NflPositionTypes.LG.ToString(), mikeEvans);

            //Assert
            Assert.Equal(backups, new List<PlayerDto>());
        }

        [Fact]
        public void ThrowsGetBackups_When_InvalidPosition()
        {
            //Arrange
            var tomBrady = new PlayerDto { Name = "Tom Brady", Number = 12 };

            var teamDepthChart = new TeamDepthChart(1);

            using var mock = AutoMock.GetStrict();
            mock.Mock<IRepository>().Setup(x => x.GetTeamDepthChart(It.IsInRange(1, 32, Moq.Range.Inclusive))).Returns(teamDepthChart);
            var manager = mock.Create<NflDepthChartManager>();

            //Act
            //Assert
            var exception = Record.Exception(() => manager.GetBackups(NflPositionTypes.RWR.ToString(), tomBrady));
            Assert.NotNull(exception);
        }

        [Fact]
        public void GetFullDepthChart_When_DepthChartExist()
        {
            //Arrange
            var blaineGabbert = new PlayerDto { Name = "Blaine Gabbert", Number = 11 };
            var scottMiller = new PlayerDto { Name = "Scott Miller", Number = 10 };
            var mikeEvans = new PlayerDto { Name = "Mike Evans", Number = 13 };

            var teamDepthChart = new TeamDepthChart(1);

            using var mock = AutoMock.GetStrict();
            mock.Mock<IRepository>().Setup(x => x.GetTeamDepthChart(It.IsInRange(1, 32, Moq.Range.Inclusive))).Returns(teamDepthChart);
            var manager = mock.Create<NflDepthChartManager>();

            //Act
            manager.AddPlayerToDepthChart(NflPositionTypes.LG.ToString(), blaineGabbert, 0);
            manager.AddPlayerToDepthChart(NflPositionTypes.LG.ToString(), scottMiller, 1);
            manager.AddPlayerToDepthChart(NflPositionTypes.LG.ToString(), mikeEvans, 2);

            var depthChart = manager.GetFullDepthChart();

            //Assert
            Assert.Contains(depthChart, x => x.Key == NflPositionTypes.LG.ToString());
            Assert.True(depthChart[NflPositionTypes.LG.ToString()].Count == 3);
            Assert.DoesNotContain(depthChart, x => x.Key == NflPositionTypes.RWR.ToString());
        }
    }
}