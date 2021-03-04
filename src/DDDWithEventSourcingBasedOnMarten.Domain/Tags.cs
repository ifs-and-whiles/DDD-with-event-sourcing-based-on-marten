using System.Collections.Generic;
using System.Linq;
using Billy.CodeReadability;

namespace DDDWithEventSourcingBasedOnMarten.Domain
{
    public class Tags : Value<Tags>
    {
        public List<Tag> Value { get; }

        public Tags(IEnumerable<Tag> value)
        {
            Value = value?.ToList() ?? new List<Tag>();
        }
        
        public static Tags operator + (Tags baseTags, Tags newTags)
        {
            var newTagsList = new List<Tag>();

            newTagsList.AddRange(baseTags.Value);
            newTagsList.AddRange(newTags.Value);

            return new Tags(newTagsList);
        }

        public static Tags operator - (Tags baseTags, Tags tagsToRemove)
        {
            var newTagsList = new List<Tag>();

            newTagsList.AddRange(baseTags.Value);

            newTagsList.RemoveAll(p => tagsToRemove.Value.Contains(p.Value));

            return new Tags(newTagsList);
        }

        public Either<TagsCanBeAssigned, AtLeastOneTagAlreadyAssigned> CanAssign(Tags tagsToAssigned)
        {
            var alreadyAssignedTags = Value.Where(t => tagsToAssigned.Value.Contains(t)).ToList();

            if (alreadyAssignedTags.Any())
                return new AtLeastOneTagAlreadyAssigned(alreadyAssignedTags);

            return new TagsCanBeAssigned();
        }

        public static Tags From(List<Tag> tags) => new Tags(tags);
        public static Tags From(List<string> tags) => new Tags(tags?.Select(t=> new Tag(t)).ToList());
        public static Tags From(params string[] tags) => new Tags(tags?.Select(t=> new Tag(t)).ToList());

        public List<string> ToStringList() => Value.Select(x => x.Value).ToList();

        public Either<TagsCanBeUnassigned, AtLeastOneTagIsNotAlreadyAssigned> CanUnassign(Tags tagsToUnassign)
        {
            var notExistingTags = tagsToUnassign.Value.Where(t => !Value.Contains(t)).ToList();

            if (notExistingTags.Any())
                return new AtLeastOneTagIsNotAlreadyAssigned(notExistingTags);

            return new TagsCanBeUnassigned();
        }

        public override string ToString() =>
            string.Join(",", Value.Select(x => x.Value));
    }

    public class TagsCanBeUnassigned
    {
 
    }

    public class AtLeastOneTagIsNotAlreadyAssigned
    {
        public AtLeastOneTagIsNotAlreadyAssigned(List<Tag> notExistingTags)
        {
            NotExistingTags = notExistingTags;
        }

        public List<Tag> NotExistingTags { get; set; }

        public override string ToString() =>
            string.Join(" ", NotExistingTags.Select(x => x.Value));
    }
    public class AtLeastOneTagAlreadyAssigned
    {
        public AtLeastOneTagAlreadyAssigned(List<Tag> duplicatedTags)
        {
            DuplicatedTags = duplicatedTags;
        }


        public List<Tag> DuplicatedTags { get; set; }

        public override string ToString() =>
            string.Join(",", DuplicatedTags.Select(x => x.Value));
    }

    public class TagsCanBeAssigned
    {

    }
    public class Tag : Value<Tag>
    {
        public string Value { get; }

        public Tag(string value)
        {
            Validate(value);
            Value = value;
        }

        private void Validate(string value)
        {
            if(string.IsNullOrWhiteSpace(value))
                throw new DomainException(
                    $"Tag '{value}' cannot be empty string or white space", 
                    ErrorCodes.InvalidTag);
        }

        public static Tag From(string tag) => new Tag(tag);

        public static implicit operator string(Tag value) => value.Value;
        public static implicit operator Tag(string value) => new Tag(value);

    }
}
