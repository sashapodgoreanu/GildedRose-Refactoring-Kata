namespace GildedRoseKata;

public class ConjuredItemUpdater : IItemUpdater
{
  public void UpdateQuality(Item item)
  {
    item.SellIn -= 1;
    if (item.Quality > 0)
    {
      item.Quality -= 2;
    }
  }
}
