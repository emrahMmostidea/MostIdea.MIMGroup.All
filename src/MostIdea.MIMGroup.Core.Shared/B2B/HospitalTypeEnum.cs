namespace MostIdea.MIMGroup.B2B
{
    /// <summary>
    /// Klinik ve bayi kullanıcıları da hastane olarak değerlendirilmekte ve bu kayıtlar için default HospitalGroup eklenmekte.
    /// List methodlarında ayrım yapabilmek için kullanılmaktadır.
    /// </summary>
    public enum HospitalTypeEnum
    {
        Hospital = 1,

        Clinic = 2,

        Dealer = 3
    }
}