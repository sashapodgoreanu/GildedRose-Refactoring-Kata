using GildedRoseKata;

public class NormalItemUpdater : IItemUpdater
{
  public void UpdateQuality(Item item)
  {
    item.SellIn -= 1;
    if (item.Quality > 0)
    {
      item.Quality -= 1;
    }

    if (item.SellIn < 0)
    {
      if (item.Quality > 0)
      {
        item.Quality -= 1;
      } // Once sell by date has passed, Quality degrades twice as fast
    }
  }
}