namespace Raffly
{
    class EmailAddress : Contact
    {
        public string Email { get; set; }

        public override string ToString()
        {
            return $"{Email}";
        }
    }
}