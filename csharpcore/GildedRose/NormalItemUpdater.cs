using GildedRoseKata;

public class NormalItemUpdater : IItemUpdater
{
    public void UpdateQuality(Item item)
    {
        // Decrease SellIn
        item.SellIn -= 1;

        // Quality decreases by 1
        if (item.Quality > 0)
        {
            item.Quality -= 1;
        }

        // Once the sell by date has passed, Quality degrades twice as fast
        if (item.SellIn < 0 && item.Quality > 0)
        {
            item.Quality -= 1;
        }
    }
}