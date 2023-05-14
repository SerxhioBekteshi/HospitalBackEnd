using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Repository.Configuration;

public class TimeZoneConfiguration : IEntityTypeConfiguration<Entities.Models.TimeZoneDetails>
{
    public void Configure(EntityTypeBuilder<Entities.Models.TimeZoneDetails> builder)
    {
        builder.HasData(
            new Entities.Models.TimeZoneDetails
            {
                Id = 1,
                Abbreviation = "CET",
                Name = "Central European Time",
                GmtOffset = "GMT+1"
            }
        );
    }
}