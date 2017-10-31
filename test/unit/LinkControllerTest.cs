using System;
using System.Collections.Generic;
using System.Linq;
using FakeItEasy;
using src.Controllers;
using src.DbModels;
using Xunit;

namespace test.unit.controller
{
	public class LinkControllerTest
    {
		private LinkController controller;
		private List<InterestingLink> links = new List<InterestingLink>();

		public LinkControllerTest()
		{
			var fakeDbContext = A.Fake<InterestingLinkContext>();
			controller = new LinkController(fakeDbContext);
			links.Add(new InterestingLink
			{
				Url = "http://example.com",
				CreatedAt = DateTime.UtcNow,
				Id = "1"
			});
			A.CallTo(() => fakeDbContext.GetAllLinks()).Returns(links);
		}

		[Fact]
        public void GetLinks_returnsLinks()
        {
	
			var result = controller.Get();
            Assert.Equal(links, result);
        }
    }
}