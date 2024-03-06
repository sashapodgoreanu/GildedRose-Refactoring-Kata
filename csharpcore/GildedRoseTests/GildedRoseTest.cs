using System.Collections.Generic;
using GildedRoseKata;
using NUnit.Framework;

namespace GildedRoseTests;

public class GildedRoseTest
{
    [Test]
    public void Foo()
    {
        var items = new List<Item> { new Item { Name = "foo", SellIn = 0, Quality = 0 } };
        var app = new GildedRose(items);
        app.UpdateQuality();
        Assert.That(items[0].Name, Is.EqualTo("fixme"));
    }


    // I will Start by writing tests for the existing behaviors before introducing "Conjured" items. 
    // This ensures that when refactor, i do not alter the intended functionality.
    

    /// <summary>
    /// This test the behavior of a normal item: The quality degrades by one and the sellIn decreases by one
    /// </summary>
    [Test]
    public void NormalItem_QualityDecreasesByOne()
    {
        var items = new List<Item> { new Item { Name = "Normal", SellIn = 10, Quality = 20 } };
        var app = new GildedRose(items);
        app.UpdateQuality();

        Assert.That(items[0].SellIn, Is.EqualTo(9));
        Assert.That(items[0].Quality, Is.EqualTo(19));
    }

    /// <summary>
    /// This test the behavior of a Aged Brie: increases in quality as it ages
    /// </summary>
    [Test]
    public void AgedBrie_IncreasesInQualityOverTime()
    {
        var items = new List<Item> { new Item { Name = "Aged Brie", SellIn = 2, Quality = 0 } };
        var app = new GildedRose(items);
        app.UpdateQuality();

        Assert.That(items[0].SellIn, Is.EqualTo(1));
        Assert.That(items[0].Quality, Is.EqualTo(1));
    }

    /// <summary>
    /// This test the behavior of a Sulfuras: never decreases in quality or sell-in
    /// </summary>

    [Test]
    public void Sulfuras_DoesNotChange()
    {
        var items = new List<Item> { new Item { Name = "Sulfuras, Hand of Ragnaros", SellIn = 0, Quality = 80 } };
        var app = new GildedRose(items);
        app.UpdateQuality();

        Assert.That(items[0].SellIn, Is.EqualTo(0));
        Assert.That(items[0].Quality, Is.EqualTo(80));
    }

    [Test]
    public void BackstagePasses_IncreaseInQualityAsConcertApproaches_ButDropsToZeroAfterConcert()
    {
        var items = new List<Item>
        {
            new Item { Name = "Backstage passes to a TAFKAL80ETC concert", SellIn = 15, Quality = 20 },
            new Item { Name = "Backstage passes to a TAFKAL80ETC concert", SellIn = 10, Quality = 45 },
            new Item { Name = "Backstage passes to a TAFKAL80ETC concert", SellIn = 5, Quality = 45 },
            new Item { Name = "Backstage passes to a TAFKAL80ETC concert", SellIn = 0, Quality = 45 }
        };
        var app = new GildedRose(items);

        app.UpdateQuality();

        Assert.That(items[0].SellIn, Is.EqualTo(14));
        Assert.That(items[0].Quality, Is.EqualTo(21));

        Assert.That(items[1].SellIn, Is.EqualTo(9));
        Assert.That(items[1].Quality, Is.EqualTo(47));

        Assert.That(items[2].SellIn, Is.EqualTo(4));
        Assert.That(items[2].Quality, Is.EqualTo(48));

        Assert.That(items[3].SellIn, Is.EqualTo(-1));
        Assert.That(items[3].Quality, Is.EqualTo(0)); // Quality drops to 0 after the concert
    }

     [Test]
        public void Quality_DegradesTwiceAsFast_WhenSellByDateHasPassed()
        {
            // Arrange: Create a list of one "Normal" item with SellIn date passed and non-zero Quality
            var items = new List<Item> { new Item { Name = "Normal", SellIn = 0, Quality = 10 } };
            var app = new GildedRose(items);

            // Act: Update the quality of the items
            app.UpdateQuality();

            // Assert: The quality of the "Normal" item should degrade by 2
            Assert.That(items[0].Quality, Is.EqualTo(8), "Quality degrades by 2 when SellIn has passed");
        }

        [Test]
        public void Quality_DegradesTwiceAsFast_AfterSellByDateHasPassed()
        {
            // Arrange: Create a "Normal" item with SellIn date just passed and non-zero Quality
            var items = new List<Item> { new Item { Name = "Normal", SellIn = -1, Quality = 10 } };
            var app = new GildedRose(items);

            // Act: Update the quality of the items
            app.UpdateQuality();

            // Assert: The quality of the "Normal" item should degrade by 2, adhering to the rule
            Assert.That(items[0].Quality, Is.EqualTo(8), "Quality degrades twice as fast after SellIn date has passed");
        }


        public static IEnumerable<TestCaseData> ItemQualityTestCases()
{
    yield return new TestCaseData(new Item { Name = "Normal", SellIn = 1, Quality = 1 });
    yield return new TestCaseData(new Item { Name = "Aged Brie", SellIn = 1, Quality = 0 });
    yield return new TestCaseData(new Item { Name = "Backstage passes to a TAFKAL80ETC concert", SellIn = 1, Quality = 1 });
    yield return new TestCaseData(new Item { Name = "Sulfuras, Hand of Ragnaros", SellIn = 1, Quality = 80 });
    yield return new TestCaseData(new Item { Name = "Conjured Mana Cake", SellIn = 1, Quality = 1 });
    // The above line assumes you would add logic to handle "Conjured" items properly.
}

[Test, TestCaseSource(nameof(ItemQualityTestCases))]
public void ItemQuality_NeverGoesNegative(Item item)
{
    var app = new GildedRose(new List<Item> { item });

    // Act: Update the quality of the item sufficiently many times to potentially deplete quality
    for (int i = 0; i < 10; ++i) // This ensures we've gone past any sell-by dates
    {
        app.UpdateQuality();
    }

    // Assert: Item's Quality is not negative
    Assert.That(item.Quality, Is.GreaterThanOrEqualTo(0), $"Item '{item.Name}' should not have negative Quality.");
}

}