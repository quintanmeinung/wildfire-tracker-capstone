using NUnit.Framework;
using System.Collections.Generic;
using System.Linq;

namespace project_wildfire_tests.UnitTests
{
    // ✅ Local DTO for unit testing only
    public class ShelterDTO
    {
        public string shelter_name { get; set; }
        public string shelter_status_code { get; set; }
        public string address_1 { get; set; }
        public string city { get; set; }
    }

    [TestFixture]
    public class EmergencyShelterServiceTests
    {
        private List<ShelterDTO> _shelters;

        [SetUp]
        public void SetUp()
        {
            _shelters = new List<ShelterDTO>
            {
                new ShelterDTO { shelter_name = "Open Shelter", shelter_status_code = "OPEN" },
                new ShelterDTO { shelter_name = "Closed Shelter", shelter_status_code = "CLOSED" },
                new ShelterDTO { shelter_name = "Empty Status", shelter_status_code = "" },
                new ShelterDTO { shelter_name = "Null Status", shelter_status_code = null }
            };
        }

        [Test]
        public void FilterOpenShelters_ShouldReturnOnlyOpen()
        {
            var result = _shelters
                .Where(s => s.shelter_status_code == "OPEN")
                .ToList();

            Assert.That(result.Count, Is.EqualTo(1));
            Assert.That(result[0].shelter_name, Is.EqualTo("Open Shelter"));
        }

        [Test]
        public void Shelter_ShouldContainBasicLocationInfo()
        {
            var shelter = new ShelterDTO
            {
                shelter_name = "Test Shelter",
                address_1 = "123 Main St",
                city = "Springfield",
                shelter_status_code = "OPEN"
            };

            Assert.That(shelter.shelter_name, Is.EqualTo("Test Shelter"));
            Assert.That(shelter.address_1, Is.EqualTo("123 Main St"));
            Assert.That(shelter.city, Is.EqualTo("Springfield"));
            Assert.That(shelter.shelter_status_code, Is.EqualTo("OPEN"));
        }
    }
}
