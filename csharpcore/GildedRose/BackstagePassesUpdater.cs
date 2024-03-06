namespace GildedRoseKata;

public class BackstagePassesUpdater : IItemUpdater
{
  public void UpdateQuality(Item item)
  {
    // Quality increases by 1
    item.Quality += 1;

    // Quality increases by 1 if SellIn is less than 11
    if (item.SellIn < 11)
    {
      item.Quality += 1;
    }

    // Quality increases by 1 if SellIn is less than 6
    if (item.SellIn < 6)
    {
      item.Quality += 1;
    }

    // Quality drops to 0 after the concert
    if (item.SellIn < 0)
    {
      item.Quality = 0;
    }

    // Decrease SellIn
    item.SellIn -= 1;
  }
}