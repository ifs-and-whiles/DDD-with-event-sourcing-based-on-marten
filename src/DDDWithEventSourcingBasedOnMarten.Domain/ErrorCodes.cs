namespace DDDWithEventSourcingBasedOnMarten.Domain
{
    public class ErrorCodes
    {
        public const string ExpenseAlreadyExists = "ExpenseAlreadyExists";
        public const string ExpenseStreamIdInvalid = "ExpenseStreamIdInvalid";
        public const string InvalidExpenseAmount = "InvalidExpenseAmount";
        public const string InvalidExpenseDate = "InvalidExpenseDate";
        public const string InvalidCreationDate = "InvalidCreationDate";
        public const string InvalidCategoryId = "InvalidCategoryId";
        public const string InvalidReceiptId = "InvalidReceiptId";
        public const string InvalidUserId = "InvalidUserId";
        public const string InvalidTag = "InvalidTag";
        public const string InvalidExpenseTitle = "InvalidExpenseTitle";
        public const string InvalidExpenseQuantity = "InvalidExpenseQuantity";
        public const string InvalidExpenseUnitPrice = "InvalidExpenseUnitPrice";
        public const string AtLeastOneTagFromTagsToAssignIsAlreadyAssigned = "AtLeastOneTagFromTagsToAssignIsAlreadyAssigned";
        public const string AtLeastOnTagFromTagsToUnassignWasNotAssigned = "AtLeastOnTagFromTagsToUnassignWasNotAssigned";
    }
}
