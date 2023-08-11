# Cpop Helper
This is my first "real" programming project, adding a bunch of new triggers and entities into Celeste. Those are mostly intended for use in personal projects, but as they might end up useful to others, I am putting them into a public helper. The helper is available for use in the game [here](https://gamebanana.com/mods/434438).

My coding skills are beyond terrible, so I apologize for anyone who dares look at the code. Feel free to reuse it, as long as you credit appropriately. Use at your own risk.

All feedback is welcome and appreciated, including suggestions or feature requests (always open for ideas, can't promise I will implement them), or feedback on the code (I am unlikely to rewrite existing code unless it breaks, but I am happy to learn more)

## Currently includes:

### Entities
 - **Cpop Block**: Adds a customizable leniency for performing corner pops.
 - **Quiz Controller**: Displays randomly chosen set of answers (TODO: and questions) for the player to pick from.
 - **HD Graphic** (*potentially unstable feature!*): Render an arbitrary high resolution graphic in-game.

### Triggers
 - **Set Subpixel Trigger**: Sets the subpixel value of the player to a prescribed value.
 - **Check Subpixel Trigger**: Performs an action depending on whether the player's subpixel value lies within a prescribed range.
 - **Quiz Answer Trigger**: Performs an action depending on whether the player's choice of an answer was correct (as determined by the Quiz Controller).

## Huge Thanks
When I began this project, I had very little programming experience, and absolutely none with C#, especially with code modding for Celeste. This mod would not exist without the help of some wonderful people in the Celeste Discord server. In particular I would like to thank:
 - Vividescence, for encouragement in starting this project,
 - Kalobi, SSM24, Microlith57, XMinty7, woofdoggo for technical help.

Additional thanks to
 - Vickyy, for (jokingly) suggesting the name for the helper, and requesting Theo Jelly.
 - Aurora Evalon, for coming up with a [project](https://gamebanana.com/mods/439982) which lead me to develop the quizzes, and requesting HD Graphic.

![catpop](https://cdn.discordapp.com/emojis/971378373790162984.gif)
