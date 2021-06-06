using System;

namespace Property_inventory.Entities
{
    public class MOL
    {
        public int Id { get; set; }
        public string FullName { get; set; }
        public int PositionId { get; set; }
        public MOLPosition Position { get; set; }
        public int PersonnelNumber { get; set; }
        public string ShortFullName => ShortingFullName(FullName);

        private string ShortingFullName(string fullName)
        {
            var FIO = fullName.Split(' ');
            for (var i = 1; i < FIO.Length; i++)
            {
                FIO[i] = FIO[i][0] + ".";
            }

            return String.Join(" ", FIO);
        }
    }
}
