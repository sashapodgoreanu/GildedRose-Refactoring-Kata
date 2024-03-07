namespace GildedRoseKata;

public class BackstagePassUpdater : IItemUpdater
{
  public void UpdateQuality(Item item)
  {
    // Increase quality until the concert date limits
    if (item.SellIn > 10)
    {
      item.Quality += 1;
    }
    else if (item.SellIn > 5)
    {
      // 10 days or less
      item.Quality += 2;
    }
    else if (item.SellIn > 0)
    {
      // 5 days or less
      item.Quality += 3;
    }
    else
    {
      // Concert has passed
      item.Quality = 0;
    }

    // Adjust sell-in value
    item.SellIn -= 1;

    // Ensure Quality cap of 50 is not exceeded before the concert
    if (item.Quality > 50)
    {
      item.Quality = 50;
    }
  }
}