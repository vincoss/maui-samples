
## Resources
https://github.com/dotnet/Microsoft.Maui.Graphics.Controls

## 




picker view sample
	single
	multiple
	see xamarin samples

Shell
<Shell.ItemTemplate>
        <DataTemplate>
            <GridLayout ColumnDefinitions="0.12*,0.88*" Padding="5">
                <Label GridLayout.Column="0" 
                       Text="{x:Static co:MaterialDesignIconConstants.Home}" 
                       FontFamily="MaterialDesign" 
                       FontSize="Title" 
                       />
                <Label GridLayout.Column="1"
                       Text="{Binding Title}"
                       VerticalTextAlignment="Center" />
            </GridLayout>
        </DataTemplate>
    </Shell.ItemTemplate>
    shell item selected item
        backgroundColour change
        icon colour change


SwipeView or FlyOut full screen on Right side as a menu

collection view
CollectionView grouping sample groups (Popular, Group A, Group B)
https://docs.microsoft.com/en-us/xamarin/xamarin-forms/user-interface/collectionview/grouping




  <Grid
            ColumnDefinitions="*"
            RowDefinitions="{OnIdiom Phone='100,*', Default='100,*,0'}"
            >
 <VerticalStackLayout Spacing="8">

# navigation sample
Home
Boards
Folders
My Cards
Settings
Help
About

## Icons
Home
Folder
Board
My Card
Settings
Help
About

Search
Done, check
Times
Ellipsis Horizontal
Ellipsis Vertical
+ Add
Edit
Delete
Template Board
Angle Left
Close
Clone
Share

  


CarouselView_Samples

## Tasks
search for todo

```
entry and color picker
	name and color
	color incremental change
multiple entry (on next add new entry with ...)
	empty must remove the entry
```

## Installation

https://docs.microsoft.com/en-us/dotnet/maui/get-started/installation

### Android paths
```
C:\Program Files (x86)\Android\android-sdk\tools
C:\Users\{UserName}\AppData\Local\Android\Sdk\tools\bin
```

## Temp
find few games to review 5-6 year old (math)
find how to use those fonts in blazor
	create a sample
	https://www.dafont.com/es/woodcutter-manero.d3980?page=1
find blazor game
Sample to create
	required digit type sample
	1,2,3 (digits)
		must raise event on each digit and reach digits
		this is array and user must fill it
input
	entry
	underline
	icon