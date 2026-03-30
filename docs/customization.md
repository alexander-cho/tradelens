# Facilitating User-defined experience: customization

### Saving charts/dashboards

#### Feature proposal and plan

Currently in the chart engine (`/chart-engine`) , the existing functionality allows the user to choose a ticker symbol 
(company), then its associated metrics (currently only financials & segment/KPIs, and not ratios, margins, etc.), choose
bar or line chart, colors, quarterly vs. yearly, and adjust from/to values. This is what I refer to as base functionality. 
Today's software is characterized by its flexibility and customizability while seamlessly providing all core functions.
The way I interpreted this initially is that users, both guests and registered users of any subscription tier can use the
same function in different ways fitting their needs. In other words, core analytical capability is shared, but 
persistence and ownership become differentiated. Now this realization may sound obvious and almost trivial, but the 
implementation process can be quite involved with potential for near-infinite extensibility.

![Chart Engine Reach](https://github.com/user-attachments/assets/aeb1059f-7c92-40b6-adf4-e6477e53ccac)

Anonymous users: Be able to generate charts, Base: Create and save up to 1 chart, Pro: Save up to 10, etc.

Requirements of "saving" a chart:

Allow users to name it, e.g. "Fintech Revenues". Contain all the data needed (including UI state such as bar/line and color)
to reproduce the chart on demand.

How should we structure this data? Should it be saved client side, or should it be a separate domain entity in the backend.
Currently, we can only display one metric on the chart at a time, but in order to do multiple, and have different colors,
line or bar, and stacked or not stacked, etc., each metric "item" should have its own configs.

Another consideration: It should be a point in time thing, i.e. if the chart data is until Q3 2025, then if the user
retrieves the saved chart a year later (4 more quarters of metrics likely added), it will get the chart data as it was
when saved. This gives us a better idea of what we need.

Full, end-to-end flow to contextualize: User creates the desired chart with all customizations like the above picture, 
then clicks the "Save" button.

```csharp
public class SavedChart : BaseEntity
{
    public required User User { get; set; }
    public required string Name { get; set; }
    public string? ChartData { get; set; }  // Core entity should not depend on Infrastructure concern, i.e., JSONB column, etc.
    //public Json? ChartData { get; set; }
    public required DateTime SavedAt { get; set; }
}
```