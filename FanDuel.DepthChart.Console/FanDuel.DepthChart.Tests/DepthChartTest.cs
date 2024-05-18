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
        private readonly Team Team;
        private readonly Player TomBradyEntity;
        private readonly Player BlaineGabbertEntity;
        private readonly Player JaelonDardenEntity;
        private readonly Player ScottMillerEntity;
        private readonly Player MikeEvansEntity;

        public DepthChartTest()
        {
            Team = new() { Id = 1, Name = "Tampa Bay Buccaneers", SportId = 1 };
            TomBradyEntity = new() { Number = 12, Name = "Tom Brady", TeamId = Team.Id };
            BlaineGabbertEntity = new() { Number = 11, Name = "Blaine Gabbert", TeamId = Team.Id };
            JaelonDardenEntity = new() { Number = 1, Name = "Jaelon Darden", TeamId = Team.Id };
            ScottMillerEntity = new() { Number = 10, Name = "Scott Miller", TeamId = Team.Id };
            MikeEvansEntity = new() { Number = 13, Name = "Mike Evans", TeamId = Team.Id };
        }

        [Fact]
        public async Task AddPlayerToDepthChart_When_EmptyPosition()
        {
            //Arrange
            var tomBrady = new PlayerDto { Name = "Tom Brady", Number = 12, TeamId = Team.Id };
            var blaineGabbert = new PlayerDto { Name = "Blaine Gabbert", Number = 11, TeamId = Team.Id };

            var teamDepthChart = new TeamDepthChart(1);

            using var mock = AutoMock.GetStrict();
            mock.Mock<IRepository>().Setup(x => x.GetPlayerByTeamNumberAsync(Team.Id, tomBrady.Number)).ReturnsAsync(TomBradyEntity);
            mock.Mock<IRepository>().Setup(x => x.GetPlayerByTeamNumberAsync(Team.Id, blaineGabbert.Number)).ReturnsAsync(BlaineGabbertEntity);
            mock.Mock<IRepository>().Setup(x => x.GetTeamDepthChartAsync(It.IsInRange(1, 100, Moq.Range.Inclusive))).ReturnsAsync(teamDepthChart);
            mock.Mock<IRepository>().Setup(x => x.UpdateTeamDepthChartAsync(It.IsAny<TeamDepthChart>())).ReturnsAsync(1);
            var manager = mock.Create<NflDepthChartManager>();

            //Act
            await manager.AddPlayerToDepthChart(NflPositionTypes.LG.ToString(), tomBrady, 0);
            await manager.AddPlayerToDepthChart(NflPositionTypes.LG.ToString(), blaineGabbert, 1);
            await manager.AddPlayerToDepthChart(NflPositionTypes.C.ToString(), blaineGabbert, 1);

            //Assert
            Assert.True(teamDepthChart.Entries.Count == 2);
            Assert.True(teamDepthChart.Entries[NflPositionTypes.LG.ToString()].Count == 2);
            Assert.True(teamDepthChart.Entries[NflPositionTypes.C.ToString()].Count == 1);
        }

        [Fact]
        public async Task AddPlayerToDepthChartThrows_When_AddingDuplicatePlayersToSamePosition()
        {
            //Arrange
            var tomBrady = new PlayerDto { Name = "Tom Brady", Number = 12, TeamId = Team.Id };
            var blaineGabbert = new PlayerDto { Name = "Blaine Gabbert", Number = 11, TeamId = Team.Id };

            var teamDepthChart = new TeamDepthChart(1);

            using var mock = AutoMock.GetStrict();
            mock.Mock<IRepository>().Setup(x => x.GetPlayerByTeamNumberAsync(Team.Id, tomBrady.Number)).ReturnsAsync(TomBradyEntity);
            mock.Mock<IRepository>().Setup(x => x.GetPlayerByTeamNumberAsync(Team.Id, blaineGabbert.Number)).ReturnsAsync(BlaineGabbertEntity);
            mock.Mock<IRepository>().Setup(x => x.GetTeamDepthChartAsync(It.IsInRange(1, 100, Moq.Range.Inclusive))).ReturnsAsync(teamDepthChart);
            mock.Mock<IRepository>().Setup(x => x.UpdateTeamDepthChartAsync(It.IsAny<TeamDepthChart>())).ReturnsAsync(1);
            var manager = mock.Create<NflDepthChartManager>();

            //Act
            await manager.AddPlayerToDepthChart(NflPositionTypes.LG.ToString(), tomBrady, 0);
            await manager.AddPlayerToDepthChart(NflPositionTypes.LG.ToString(), blaineGabbert, 1);
            var exception = Record.ExceptionAsync(async () => await manager.AddPlayerToDepthChart(NflPositionTypes.LG.ToString(), blaineGabbert, 1));

            //Assert
            Assert.NotNull(exception);
        }

        [Fact]
        public async Task AddPlayerToDepthChartThrows_When_AddingInvalidPosition()
        {
            //Arrange
            var tomBrady = new PlayerDto { Name = "Tom Brady", Number = 12, TeamId = Team.Id };
            var blaineGabbert = new PlayerDto { Name = "Blaine Gabbert", Number = 11, TeamId = Team.Id };

            var teamDepthChart = new TeamDepthChart(1);

            using var mock = AutoMock.GetStrict();
            mock.Mock<IRepository>().Setup(x => x.GetPlayerByTeamNumberAsync(Team.Id, tomBrady.Number)).ReturnsAsync(TomBradyEntity);
            mock.Mock<IRepository>().Setup(x => x.GetPlayerByTeamNumberAsync(Team.Id, blaineGabbert.Number)).ReturnsAsync(BlaineGabbertEntity);
            mock.Mock<IRepository>().Setup(x => x.GetTeamDepthChartAsync(It.IsInRange(1, 100, Moq.Range.Inclusive))).ReturnsAsync(teamDepthChart);
            mock.Mock<IRepository>().Setup(x => x.UpdateTeamDepthChartAsync(It.IsAny<TeamDepthChart>())).ReturnsAsync(1);
            var manager = mock.Create<NflDepthChartManager>();

            //Act
            await manager.AddPlayerToDepthChart(NflPositionTypes.LG.ToString(), tomBrady, 0);
            await manager.AddPlayerToDepthChart(NflPositionTypes.LG.ToString(), blaineGabbert, 1);
            var exception = await Record.ExceptionAsync(() => manager.AddPlayerToDepthChart("BB", blaineGabbert, 1));

            //Assert
            Assert.NotNull(exception);
        }

        [Fact]
        public async Task AddPlayerToEndOfDepthChart_When_NoPositionProvided()
        {
            //Arrange
            var tomBrady = new PlayerDto { Name = "Tom Brady", Number = 12, TeamId = Team.Id };
            var blaineGabbert = new PlayerDto { Name = "Blaine Gabbert", Number = 11, TeamId = Team.Id };

            var teamDepthChart = new TeamDepthChart(1);

            using var mock = AutoMock.GetStrict();
            mock.Mock<IRepository>().Setup(x => x.GetPlayerByTeamNumberAsync(Team.Id, tomBrady.Number)).ReturnsAsync(TomBradyEntity);
            mock.Mock<IRepository>().Setup(x => x.GetPlayerByTeamNumberAsync(Team.Id, blaineGabbert.Number)).ReturnsAsync(BlaineGabbertEntity);
            mock.Mock<IRepository>().Setup(x => x.GetTeamDepthChartAsync(It.IsInRange(1, 100, Moq.Range.Inclusive))).ReturnsAsync(teamDepthChart);
            mock.Mock<IRepository>().Setup(x => x.UpdateTeamDepthChartAsync(It.IsAny<TeamDepthChart>())).ReturnsAsync(1);
            var manager = mock.Create<NflDepthChartManager>();

            //Act
            await manager.AddPlayerToDepthChart(NflPositionTypes.LG.ToString(), tomBrady);
            await manager.AddPlayerToDepthChart(NflPositionTypes.LG.ToString(), blaineGabbert);

            //Assert
            Assert.True(teamDepthChart.Entries[NflPositionTypes.LG.ToString()][0].Player.Number == tomBrady.Number);
            Assert.True(teamDepthChart.Entries[NflPositionTypes.LG.ToString()][1].Player.Number == blaineGabbert.Number);
        }

        [Fact]
        public async Task MoveDownOtherPlayersOnePosition_When_NewPlayerIsAdded()
        {
            //Arrange
            var tomBrady = new PlayerDto { Name = "Tom Brady", Number = 12, TeamId = Team.Id };
            var blaineGabbert = new PlayerDto { Name = "Blaine Gabbert", Number = 11, TeamId = Team.Id };
            var jaelonDarden = new PlayerDto { Name = "Jaelon Darden", Number = 1, TeamId = Team.Id };
            var scottMiller = new PlayerDto { Name = "Scott Miller", Number = 10, TeamId = Team.Id };
            var mikeEvans = new PlayerDto { Name = "Mike Evans", Number = 13, TeamId = Team.Id };

            var teamDepthChart = new TeamDepthChart(1);

            using var mock = AutoMock.GetStrict();
            mock.Mock<IRepository>().Setup(x => x.GetPlayerByTeamNumberAsync(Team.Id, tomBrady.Number)).ReturnsAsync(TomBradyEntity);
            mock.Mock<IRepository>().Setup(x => x.GetPlayerByTeamNumberAsync(Team.Id, blaineGabbert.Number)).ReturnsAsync(BlaineGabbertEntity);
            mock.Mock<IRepository>().Setup(x => x.GetPlayerByTeamNumberAsync(Team.Id, jaelonDarden.Number)).ReturnsAsync(JaelonDardenEntity);
            mock.Mock<IRepository>().Setup(x => x.GetPlayerByTeamNumberAsync(Team.Id, scottMiller.Number)).ReturnsAsync(ScottMillerEntity);
            mock.Mock<IRepository>().Setup(x => x.GetPlayerByTeamNumberAsync(Team.Id, mikeEvans.Number)).ReturnsAsync(MikeEvansEntity);
            mock.Mock<IRepository>().Setup(x => x.GetTeamDepthChartAsync(It.IsInRange(1, 100, Moq.Range.Inclusive))).ReturnsAsync(teamDepthChart);
            mock.Mock<IRepository>().Setup(x => x.UpdateTeamDepthChartAsync(It.IsAny<TeamDepthChart>())).ReturnsAsync(1);
            var manager = mock.Create<NflDepthChartManager>();

            //Act
            await manager.AddPlayerToDepthChart(NflPositionTypes.LG.ToString(), tomBrady, 0);
            await manager.AddPlayerToDepthChart(NflPositionTypes.LG.ToString(), blaineGabbert, 1);
            await manager.AddPlayerToDepthChart(NflPositionTypes.LG.ToString(), jaelonDarden, 1);
            await manager.AddPlayerToDepthChart(NflPositionTypes.LG.ToString(), scottMiller, 1);
            await manager.AddPlayerToDepthChart(NflPositionTypes.LG.ToString(), mikeEvans, 2);

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
        public async Task ThrowAddPlayerToEndOfDepthChart_When_PlayerNotFound()
        {
            //Arrange
            var mikeEvans = new PlayerDto { Name = "Mike Evans", Number = 13, TeamId = Team.Id };

            var teamDepthChart = new TeamDepthChart(1);

            using var mock = AutoMock.GetStrict();
            mock.Mock<IRepository>().Setup(x => x.GetPlayerByTeamNumberAsync(Team.Id, mikeEvans.Number)).ReturnsAsync((Player)null);
            mock.Mock<IRepository>().Setup(x => x.GetTeamDepthChartAsync(It.IsInRange(1, 100, Moq.Range.Inclusive))).ReturnsAsync(teamDepthChart);
            mock.Mock<IRepository>().Setup(x => x.UpdateTeamDepthChartAsync(It.IsAny<TeamDepthChart>())).ReturnsAsync(1);
            var manager = mock.Create<NflDepthChartManager>();

            //Act
            var exception = await Record.ExceptionAsync(() => manager.AddPlayerToDepthChart(NflPositionTypes.LG.ToString(), mikeEvans, 2));

            //Assert
            Assert.NotNull(exception);
        }

        [Fact]
        public async Task RemovePlayerFromDepthChart_When_PositionExists()
        {
            //Arrange
            var tomBrady = new PlayerDto { Name = "Tom Brady", Number = 12, TeamId = Team.Id };
            var blaineGabbert = new PlayerDto { Name = "Blaine Gabbert", Number = 11, TeamId = Team.Id };
            var jaelonDarden = new PlayerDto { Name = "Jaelon Darden", Number = 1, TeamId = Team.Id };
            var scottMiller = new PlayerDto { Name = "Scott Miller", Number = 10, TeamId = Team.Id };
            var mikeEvans = new PlayerDto { Name = "Mike Evans", Number = 13, TeamId = Team.Id };

            var lgEntries = new List<DepthChartEntry>()
            {
                new DepthChartEntry
                {
                    Player = ScottMillerEntity,
                    Position = NflPositionTypes.LG.ToString(),
                    Rank = 0
                },
                new DepthChartEntry
                {
                    Player = JaelonDardenEntity,
                    Position = NflPositionTypes.LG.ToString(),
                    Rank = 1
                },
                new DepthChartEntry
                {
                    Player = BlaineGabbertEntity,
                    Position = NflPositionTypes.LG.ToString(),
                    Rank = 2
                },
                new DepthChartEntry
                {
                    Player = MikeEvansEntity,
                    Position = NflPositionTypes.LG.ToString(),
                    Rank = 3
                },
            };
            var qbEntries = new List<DepthChartEntry>() {
                new DepthChartEntry
                {
                    Player = MikeEvansEntity,
                    Position = NflPositionTypes.QB.ToString(),
                    Rank = 0
                }
            };
            var rwrEntries = new List<DepthChartEntry>
            {
                new DepthChartEntry
                {
                    Player = MikeEvansEntity,
                    Position = NflPositionTypes.RWR.ToString(),
                    Rank = 0
                }
            };

            var teamDepthChart = new TeamDepthChart(1);
            teamDepthChart.Entries.Add(NflPositionTypes.LG.ToString(), lgEntries);
            teamDepthChart.Entries.Add(NflPositionTypes.QB.ToString(), qbEntries);
            teamDepthChart.Entries.Add(NflPositionTypes.RWR.ToString(), rwrEntries);

            using var mock = AutoMock.GetStrict();
            mock.Mock<IRepository>().Setup(x => x.GetTeamDepthChartAsync(It.IsInRange(1, 100, Moq.Range.Inclusive))).ReturnsAsync(teamDepthChart);
            var manager = mock.Create<NflDepthChartManager>();

            //Act
            await manager.RemovePlayerFromDepthChart(NflPositionTypes.RWR.ToString(), mikeEvans);
            await manager.RemovePlayerFromDepthChart(NflPositionTypes.LG.ToString(), mikeEvans);

            //Assert
            Assert.DoesNotContain(teamDepthChart.Entries[NflPositionTypes.RWR.ToString()], x => x.Player.Number == mikeEvans.Number);
            Assert.DoesNotContain(teamDepthChart.Entries[NflPositionTypes.LG.ToString()], x => x.Player.Number == mikeEvans.Number);
            Assert.Contains(teamDepthChart.Entries[NflPositionTypes.QB.ToString()], x => x.Player.Number == mikeEvans.Number);
        }

        [Fact]
        public async Task MoveUpOtherPlayersOnePosition_When_APlayerIsRemoved()
        {
            //Arrange
            var blaineGabbert = new PlayerDto { Name = "Blaine Gabbert", Number = 11, TeamId = Team.Id };
            var jaelonDarden = new PlayerDto { Name = "Jaelon Darden", Number = 1, TeamId = Team.Id };
            var scottMiller = new PlayerDto { Name = "Scott Miller", Number = 10, TeamId = Team.Id };
            var mikeEvans = new PlayerDto { Name = "Mike Evans", Number = 13, TeamId = Team.Id };

            var lgEntries = new List<DepthChartEntry>()
            {
                new DepthChartEntry
                {
                    Player = BlaineGabbertEntity,
                    Position = NflPositionTypes.LG.ToString(),
                    Rank = 0
                },
                new DepthChartEntry
                {
                    Player = ScottMillerEntity,
                    Position = NflPositionTypes.LG.ToString(),
                    Rank = 1
                },
                new DepthChartEntry
                {
                    Player = MikeEvansEntity,
                    Position = NflPositionTypes.LG.ToString(),
                    Rank = 2
                },
                new DepthChartEntry
                {
                    Player = JaelonDardenEntity,
                    Position = NflPositionTypes.LG.ToString(),
                    Rank = 3
                },
            };
            var teamDepthChart = new TeamDepthChart(1);
            teamDepthChart.Entries.Add(NflPositionTypes.LG.ToString(), lgEntries);

            using var mock = AutoMock.GetStrict();
            mock.Mock<IRepository>().Setup(x => x.GetTeamDepthChartAsync(It.IsInRange(1, 32, Moq.Range.Inclusive))).ReturnsAsync(teamDepthChart);
            var manager = mock.Create<NflDepthChartManager>();

            //Act
            await manager.RemovePlayerFromDepthChart(NflPositionTypes.LG.ToString(), jaelonDarden);

            //Assert
            Assert.True(teamDepthChart.Entries[NflPositionTypes.LG.ToString()][0].Player.Number == blaineGabbert.Number);
            Assert.True(teamDepthChart.Entries[NflPositionTypes.LG.ToString()][0].Rank == 0);
            Assert.True(teamDepthChart.Entries[NflPositionTypes.LG.ToString()][1].Player.Number == scottMiller.Number);
            Assert.True(teamDepthChart.Entries[NflPositionTypes.LG.ToString()][1].Rank == 1);
            Assert.True(teamDepthChart.Entries[NflPositionTypes.LG.ToString()][2].Player.Number == mikeEvans.Number);
            Assert.True(teamDepthChart.Entries[NflPositionTypes.LG.ToString()][2].Rank == 2);
        }

        [Fact]
        public async Task ReturnRemovedPlayer_When_PlayerExistInGivenPosition()
        {
            //Arrange
            var jaelonDarden = new PlayerDto { Name = "Jaelon Darden", Number = 1, TeamId = Team.Id };

            var lgEntries = new List<DepthChartEntry>()
            {
                new DepthChartEntry
                {
                    Player = BlaineGabbertEntity,
                    Position = NflPositionTypes.LG.ToString(),
                    Rank = 0
                },
                new DepthChartEntry
                {
                    Player = JaelonDardenEntity,
                    Position = NflPositionTypes.LG.ToString(),
                    Rank = 1
                },
            };
            var teamDepthChart = new TeamDepthChart(1);
            teamDepthChart.Entries.Add(NflPositionTypes.LG.ToString(), lgEntries);

            using var mock = AutoMock.GetStrict();
            mock.Mock<IRepository>().Setup(x => x.GetTeamDepthChartAsync(It.IsInRange(1, 32, Moq.Range.Inclusive))).ReturnsAsync(teamDepthChart);
            var manager = mock.Create<NflDepthChartManager>();

            //Act
            var removedPlayers = await manager.RemovePlayerFromDepthChart(NflPositionTypes.LG.ToString(), jaelonDarden);

            //Assert
            Assert.Contains(removedPlayers, x => x.Number == jaelonDarden.Number);
        }

        [Fact]
        public async Task ReturnEmptyList_When_PlayerDoesNotExistInGivenPosition()
        {
            //Arrange
            var jaelonDarden = new PlayerDto { Name = "Jaelon Darden", Number = 1, TeamId = Team.Id };

            var lgEntries = new List<DepthChartEntry>()
            {
                new DepthChartEntry
                {
                    Player = BlaineGabbertEntity,
                    Position = NflPositionTypes.LG.ToString(),
                    Rank = 0
                },
                new DepthChartEntry
                {
                    Player = MikeEvansEntity,
                    Position = NflPositionTypes.LG.ToString(),
                    Rank = 1
                },
            };
            var teamDepthChart = new TeamDepthChart(1);
            teamDepthChart.Entries.Add(NflPositionTypes.LG.ToString(), lgEntries);

            using var mock = AutoMock.GetStrict();
            mock.Mock<IRepository>().Setup(x => x.GetTeamDepthChartAsync(It.IsInRange(1, 32, Moq.Range.Inclusive))).ReturnsAsync(teamDepthChart);
            var manager = mock.Create<NflDepthChartManager>();

            //Act
            var removedPlayers = await manager.RemovePlayerFromDepthChart(NflPositionTypes.LG.ToString(), jaelonDarden);

            //Assert
            Assert.Equal(removedPlayers, []);
        }

        [Fact]
        public async Task Throws_When_InvalidPosition()
        {
            //Arrange
            var tomBrady = new PlayerDto { Name = "Tom Brady", Number = 12 };

            using var mock = AutoMock.GetStrict();
            var manager = mock.Create<NflDepthChartManager>();

            //Act
            var exception = await Record.ExceptionAsync(() => manager.RemovePlayerFromDepthChart(NflPositionTypes.RWR.ToString(), tomBrady));

            //Assert
            Assert.NotNull(exception);
        }

        [Fact]
        public async Task GetBackups_When_BackupsExist()
        {
            //Arrange
            var blaineGabbert = new PlayerDto { Name = "Blaine Gabbert", Number = 11 };
            var scottMiller = new PlayerDto { Name = "Scott Miller", Number = 10 };
            var mikeEvans = new PlayerDto { Name = "Mike Evans", Number = 13 };

            var lgEntries = new List<DepthChartEntry>()
            {
                new DepthChartEntry
                {
                    Player = BlaineGabbertEntity,
                    Position = NflPositionTypes.LG.ToString(),
                    Rank = 0
                },
                new DepthChartEntry
                {
                    Player = ScottMillerEntity,
                    Position = NflPositionTypes.LG.ToString(),
                    Rank = 1
                },
                new DepthChartEntry
                {
                    Player = MikeEvansEntity,
                    Position = NflPositionTypes.LG.ToString(),
                    Rank = 2
                }
            };

            var teamDepthChart = new TeamDepthChart(1);
            teamDepthChart.Entries.Add(NflPositionTypes.LG.ToString(), lgEntries);

            using var mock = AutoMock.GetStrict();
            mock.Mock<IRepository>().Setup(x => x.GetTeamDepthChartAsync(It.IsInRange(1, 32, Moq.Range.Inclusive))).ReturnsAsync(teamDepthChart);
            var manager = mock.Create<NflDepthChartManager>();

            //Act
            var backups = await manager.GetBackups(NflPositionTypes.LG.ToString(), blaineGabbert);

            //Assert
            Assert.Contains(backups, x => x.Number == scottMiller.Number);
            Assert.Contains(backups, x => x.Number == mikeEvans.Number);
        }

        [Fact]
        public async Task GetEmptyList_When_GivenPlayerDoesNotExist()
        {
            //Arrange
            var tomBrady = new PlayerDto { Name = "Tom Brady", Number = 12 };
            var blaineGabbert = new PlayerDto { Name = "Blaine Gabbert", Number = 11 };
            var scottMiller = new PlayerDto { Name = "Scott Miller", Number = 10 };
            var mikeEvans = new PlayerDto { Name = "Mike Evans", Number = 13 };

            var lgEntries = new List<DepthChartEntry>()
            {
                new DepthChartEntry
                {
                    Player = BlaineGabbertEntity,
                    Position = NflPositionTypes.LG.ToString(),
                    Rank = 0
                },
                new DepthChartEntry
                {
                    Player = ScottMillerEntity,
                    Position = NflPositionTypes.LG.ToString(),
                    Rank = 1
                },
                new DepthChartEntry
                {
                    Player = MikeEvansEntity,
                    Position = NflPositionTypes.LG.ToString(),
                    Rank = 2
                }
            };

            var teamDepthChart = new TeamDepthChart(1);
            teamDepthChart.Entries.Add(NflPositionTypes.LG.ToString(), lgEntries);

            using var mock = AutoMock.GetStrict();
            mock.Mock<IRepository>().Setup(x => x.GetTeamDepthChartAsync(It.IsInRange(1, 32, Moq.Range.Inclusive))).ReturnsAsync(teamDepthChart);
            var manager = mock.Create<NflDepthChartManager>();

            //Act
            var backups = await manager.GetBackups(NflPositionTypes.LG.ToString(), tomBrady);

            //Assert
            Assert.Equal(backups, []);
        }

        [Fact]
        public async Task GetEmptyList_When_NoBackups()
        {
            //Arrange
            var blaineGabbert = new PlayerDto { Name = "Blaine Gabbert", Number = 11 };
            var scottMiller = new PlayerDto { Name = "Scott Miller", Number = 10 };
            var mikeEvans = new PlayerDto { Name = "Mike Evans", Number = 13 };

            var lgEntries = new List<DepthChartEntry>()
            {
                new DepthChartEntry
                {
                    Player = BlaineGabbertEntity,
                    Position = NflPositionTypes.LG.ToString(),
                    Rank = 0
                },
                new DepthChartEntry
                {
                    Player = ScottMillerEntity,
                    Position = NflPositionTypes.LG.ToString(),
                    Rank = 1
                },
                new DepthChartEntry
                {
                    Player = MikeEvansEntity,
                    Position = NflPositionTypes.LG.ToString(),
                    Rank = 2
                }
            };

            var teamDepthChart = new TeamDepthChart(1);
            teamDepthChart.Entries.Add(NflPositionTypes.LG.ToString(), lgEntries);

            using var mock = AutoMock.GetStrict();
            mock.Mock<IRepository>().Setup(x => x.GetTeamDepthChartAsync(It.IsInRange(1, 32, Moq.Range.Inclusive))).ReturnsAsync(teamDepthChart);
            var manager = mock.Create<NflDepthChartManager>();

            //Act
            var backups = await manager.GetBackups(NflPositionTypes.LG.ToString(), mikeEvans);

            //Assert
            Assert.Equal(backups, []);
        }

        [Fact]
        public async Task ThrowsGetBackups_When_InvalidPosition()
        {
            //Arrange
            var tomBrady = new PlayerDto { Name = "Tom Brady", Number = 12 };

            using var mock = AutoMock.GetStrict();
            var manager = mock.Create<NflDepthChartManager>();

            //Act
            var exception = await Record.ExceptionAsync(() => manager.GetBackups(NflPositionTypes.RWR.ToString(), tomBrady));

            //Assert
            Assert.NotNull(exception);
        }

        [Fact]
        public async Task GetFullDepthChart_When_DepthChartExist()
        {
            //Arrange
            var blaineGabbert = new PlayerDto { Name = "Blaine Gabbert", Number = 11 };
            var scottMiller = new PlayerDto { Name = "Scott Miller", Number = 10 };
            var mikeEvans = new PlayerDto { Name = "Mike Evans", Number = 13 };

            var lgEntries = new List<DepthChartEntry>()
            {
                new DepthChartEntry
                {
                    Player = BlaineGabbertEntity,
                    Position = NflPositionTypes.LG.ToString(),
                    Rank = 0
                },
                new DepthChartEntry
                {
                    Player = ScottMillerEntity,
                    Position = NflPositionTypes.LG.ToString(),
                    Rank = 1
                },
                new DepthChartEntry
                {
                    Player = MikeEvansEntity,
                    Position = NflPositionTypes.LG.ToString(),
                    Rank = 2
                }
            };

            var teamDepthChart = new TeamDepthChart(1);
            teamDepthChart.Entries.Add(NflPositionTypes.LG.ToString(), lgEntries);

            using var mock = AutoMock.GetStrict();
            mock.Mock<IRepository>().Setup(x => x.GetTeamDepthChartAsync(It.IsInRange(1, 100, Moq.Range.Inclusive))).ReturnsAsync(teamDepthChart);
            var manager = mock.Create<NflDepthChartManager>();

            //Act
            var depthChart = await manager.GetFullDepthChart();

            //Assert
            Assert.Contains(depthChart, x => x.Key == NflPositionTypes.LG.ToString());
            Assert.True(depthChart[NflPositionTypes.LG.ToString()].Count == 3);
            Assert.DoesNotContain(depthChart, x => x.Key == NflPositionTypes.RWR.ToString());
        }
    }
}