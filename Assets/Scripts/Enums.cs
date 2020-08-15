using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

public enum Parameters { a }
public enum SceneTransitionType { FadeIO, }

public enum GameSceneType { Dialogue, Cook, Clean, Ending }
public enum CharacterName { None, BaekYum, Dowoon, Grandma, Gion, Mina, Eugene, Unknown }
public enum LineType { speech, enter, exit, choice, narration, result, reaction }

public enum IngredientType1 { meat, seafood, vegetable }

public enum IngredientName { none = 0, chicken, pork, beef, egg, pollack, salmon, shrimp, clam, carrot, onion, potato, tomato, any }
//public enum Meat { none = 0, chicken, pork, beef, egg }
//public enum Seafood { none = 0, pollack, salmon, shrimp, clam }
//public enum Vegetable { none = 0, carrot, onion, potato, tomato }
public enum FoodName { roastChicken, porkBarbecue, roastBeef, roastEgg, roastPollack, roastSalmon, roastShrimp, roastClam, roastPotato, samhap, friedChicken, friedPork, friedShrimp, friedOnion, frenchFries, tomatoEgg, scrambledEgg, eggRoll, friedCombo, chickenSoup, boiledPork, beefStew, tomatoEggSoup, pollackSoup, spicyStew, clamSoup, vegetableSoup, jjampong, rawMeat, rawSalmon, salad, trash }

public enum CookProcess { none = -1, boil, fry, grill, raw }

public enum Language { Korean, English }