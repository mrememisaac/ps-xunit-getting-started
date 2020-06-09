using System;
using Xunit;
using Xunit.Abstractions;

namespace GameEngine.Tests
{
    public class PlayerCharacterShould : IDisposable
    {
        private readonly PlayerCharacter _sut;
        private readonly ITestOutputHelper _output;

        public PlayerCharacterShould(ITestOutputHelper output)
        {
            _output = output;

            _output.WriteLine("Creating new PlayerCharacter");
            _sut = new PlayerCharacter();
        }

        public void Dispose()
        {
            _output.WriteLine($"Disposing PlayerCharacter {_sut.FullName}");
            //_sut.Dispose;
        }

        [Fact]
        public void BeInexperiencedWhenNew()
        {
            // Assert that player is a noob
            Assert.True(_sut.IsNoob);
        }

        [Fact]
        public void CalculateFullName()
        {
            _sut.FirstName = "Sarah";
            _sut.LastName = "Smith";

            // Assert that the full name is Sarah Smith
            Assert.Equal("Sarah Smith", _sut.FullName);
        }

        [Fact]
        public void HaveFullNameStartingWithFirstName()
        {
            _sut.FirstName = "Sarah";
            _sut.LastName = "Smith";

            // Assert that the fullname starts with Sarah
            Assert.StartsWith("Sarah", _sut.FullName);
        }


        [Fact]
        public void HaveFullNameEndingWithLastName()
        {
            _sut.LastName = "Smith";

            // Assert that the full name ends with Smith
            Assert.EndsWith("Smith", _sut.FullName);
        }

        [Fact]
        public void CalculateFullName_IgnoreCaseAssertExample()
        {
            _sut.FirstName = "SARAH";
            _sut.LastName = "SMITH";

            // Assert that the full name is Sarah Smith, ingnoring case sensitivity
            Assert.Equal("Sarah Smith", _sut.FullName, ignoreCase: true);
        }

        [Fact]
        public void CalculateFullName_SubstringAssertExample()
        {
            _sut.FirstName = "Sarah";
            _sut.LastName = "Smith";

            // Assert that the full name contains "ah Sm"
            Assert.Contains("ah Sm", _sut.FullName);
        }

        [Fact]
        public void CalculateFullNameWithTitleCase()
        {
            _sut.FirstName = "Sarah";
            _sut.LastName = "Smith";

            // Assert that the full name should macht the regex:
            // - Uppercase first character for first name, 
            // - Uppercase first character for last name
            Assert.Matches("[A-Z]{1}[a-z]+ [A-Z]{1}[a-z]+", _sut.FullName);
        }

        [Fact]
        public void StartWithDefaultHealth()
        {
            // Assert that health should be equal to 100
            Assert.Equal(100, _sut.Health);
        }

        [Fact]
        public void StartWithDefaultHealth_NotEqualExample()
        {
            // Assert that health should not be equal to 0
            Assert.NotEqual(0, _sut.Health);

        }

        [Fact]
        public void IncreaseHealthAfterSleeping()
        {
            _sut.Sleep(); // Generates an increase in health

            // Assert that health is within a range
            //Assert.True(_sut.Health >= 101 && _sut.Health <= 200);
            Assert.InRange(_sut.Health, 101, 200);

        }

        [Fact]
        public void NotHaveNicknameByDefault()
        {
            // Assert that the nickname is null
            Assert.Null(_sut.Nickname);
        }

        [Fact]
        public void HaveALongBow()
        {
            // Assert that the collection contains an item named "Long Bow"
            Assert.Contains("Long Bow", _sut.Weapons);
        }

        [Fact]
        public void NotHaveAStaffOfWonder()
        {
            // Assert that the collection does not contain an item named
            // "Staff of Wonder"
            Assert.DoesNotContain("Staff Of Wonder", _sut.Weapons);
        }

        [Fact]
        public void HaveAtLeastOneKindOfSword()
        {
            // Assert that the collection contains one item containing the 
            // word "Sword"
            Assert.Contains(_sut.Weapons, weapon => weapon.Contains("Sword"));
        }

        [Fact]
        public void HaveAllExpectedWeapons()
        {
            var expectedWeapons = new[]
            {
                 "Long Bow",
                 "Short Bow",
                 "Short Sword"
             };

            // Assert that all of the expected weapons exist in the collection
            Assert.Equal(expectedWeapons, _sut.Weapons);
        }

        [Fact]
        public void HaveNoEmptyDefaultWeapons()
        {
            // Like a for loop, checks all entries in the collection for the inner Assert
            // Assert that all weapons in the collection are not null or empty
            Assert.All(_sut.Weapons, weapon => Assert.False(string.IsNullOrWhiteSpace(weapon)));
        }

        [Fact]
        public void RaiseSleptEvent()
        {
            // If playerslept event method is raised and sleep method is called, this test will pass
            Assert.Raises<EventArgs>(
                handler => _sut.PlayerSlept += handler,
                handler => _sut.PlayerSlept -= handler,
                () => _sut.Sleep());
        }


        [Fact]
        public void RaisePropertyChangedEvent()
        {
            // Special assert used when implementing INotifyPropertyChanged
            Assert.PropertyChanged(_sut, "Health", () => _sut.TakeDamage(10));
        }
        [Theory]
        //[MemberData(nameof(ExternalHealthDamageTestData.TestData),
        //    MemberType = typeof(ExternalHealthDamageTestData))]
        [HealthDamageData]
        public void TakeDamage(int damage, int expectedHealth)
        {
            _sut.TakeDamage(damage);

            Assert.Equal(expectedHealth, _sut.Health);
        }
    }
}
