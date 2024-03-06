using System;
using GildedRoseKata;

public class AgedBrieUpdater : IItemUpdater
{
  public void UpdateQuality(Item item)
  {
    // Increase the quality as it gets older
    if (item.Quality < 50)
    {
      item.Quality += 1;
    }

    // Decrement sell-in
    item.SellIn -= 1;

    // Check if SellIn date has passed
    if (item.SellIn < 0 && item.Quality < 50)
    {
      // Once the sell by date has passed, Quality increases twice as fast
      item.Quality += 1;
    }
  }
}