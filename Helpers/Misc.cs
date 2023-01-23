namespace Catalog.Helpers
{
    public static class Misc
    {
        public static bool Contains(List<string> list1, List<string> list2)
        {
            bool result = false;
            if (list1.Count != 0 && list2.Count != 0)
            {
                for (int i = 0; i < list1.Count; i++)
                {
                    string currentValue = list1[i];
                    if (list2.Contains(currentValue))
                    {
                        result = true;
                        break;
                    }
                }
            }
            return result;
        }
    }
}
