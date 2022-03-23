namespace Sklad.Infrastructure
{
    public static class NewCount
    {
        public static int IncomeCount(int currentCount, int numberOfItems)
        {
            return currentCount + numberOfItems;
        }

        public static int OutcomeCount(int currentCount, int numberOfItems)
        {
            var res = currentCount - numberOfItems;
            if (res < 0)
                throw new ValidationException();
            return res;
        }
    }
}