using System;
using System.Collections.Generic;
using System.Linq;
using DDDWithEventSourcingBasedOnMarten.Domain;
using FluentAssertions;
using Xunit;

namespace DDDWithEventSourcingBasedOnMarten.Expenses.Domain.Tests.ValueObjects
{
    public class TagsTests
    {
        [Theory]
        [InlineData(null)]
        [InlineData("")]
        [InlineData(" ")]
        public void should_fail_validation_when_at_least_one_tag_has_incorrect_format(string tag)
        {
            var exception = Assert.Throws<DomainException>(() => new Tags(new List<Tag>(){ "test", tag }));
            exception.ErrorCode.Should().Be(ErrorCodes.InvalidTag);
        }

        [Fact]
        public void can_be_empty()
        {
            var tags = new Tags(null);
            tags.Value.Should().BeEquivalentTo(new List<string>());
        }

        [Theory]
        [InlineData("tag")]
        [InlineData("first_tag")]
        [InlineData("first-tag", "second_tag")]
        [InlineData("part1 part2")]
        public void should_pass_validation_for_correct_tags(params string [] tags)
        {
            var validateAction = new Action(() => new Tags(tags.Select(t=> new Tag(t)).ToList()));
            validateAction.Should().NotThrow();
        }

        [Fact]
        public void should_be_able_to_assign_not_existing_tags()
        {
            var tags = Tags.From("tag1", "tag2");
            var tagsToAssign = Tags.From("tag3", "tag4");

            tags.CanAssign(tagsToAssign)
                .Match(
                    tagsCanBeAssigned => tagsCanBeAssigned.Should().NotBeNull(),
                    atLeastOneTagAlreadyAssigned => throw new WrongResultException());

            tags += tagsToAssign;

            tags.Value.Should().BeEquivalentTo(new List<Tag>(){"tag1","tag2","tag3","tag4"});
        }

        [Fact]
        public void should_be_able_to_unassign_existing_tags()
        {
            var tags = Tags.From("tag1", "tag2", "tag3");
            var tagsToUnssign = Tags.From("tag1", "tag2");

            tags.CanUnassign(tagsToUnssign)
                .Match(
                    tagsCanBeUnassigned => tagsCanBeUnassigned.Should().NotBeNull(),
                    atLeastOneTagIsNotAlreadyAssigned => throw new WrongResultException());

            tags -= tagsToUnssign;

            tags.Value.Should().BeEquivalentTo(new List<Tag>() { "tag3" });
        }

        [Fact]
        public void should_not_assign_already_existing_tags()
        {
            var tags = Tags.From("tag1", "tag2");
            var tagsToAssign = Tags.From("tag1", "tag4");

            tags.CanAssign(tagsToAssign)
                .Match(
                    tagsCanBeAssigned => throw new WrongResultException(),
                    atLeastOneTagAlreadyAssigned => atLeastOneTagAlreadyAssigned.Should().NotBeNull());

            tags.Value.Should().BeEquivalentTo(new List<Tag>() { "tag1", "tag2" });
        }

        [Fact]
        public void should_not_unassign_not_existing_tags()
        {
            var tags = Tags.From("tag1", "tag2");
            var tagsToAssign = Tags.From("tag3", "tag4");

            tags.CanUnassign(tagsToAssign)
                .Match(
                    tagsCanBeUnassigned => throw new WrongResultException(),
                    atLeastOneTagIsNotAlreadyAssigned => atLeastOneTagIsNotAlreadyAssigned.Should().NotBeNull());

            tags.Value.Should().BeEquivalentTo(new List<Tag>() { "tag1", "tag2" });
        }


    }
}
