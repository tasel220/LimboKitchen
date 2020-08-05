using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

public enum Parameters { a }
public enum SceneTransitionType { FadeIO, }

public enum GameSceneType { Dialogue, Cook, Clean, Ending }
public enum CharacterName { None, BaekYum, Dowoon, Grandma, Gion, Mina, Eugene, Unknown }
public enum LineType { speech, enter, exit, choice, narration, result }

public enum IngredientType1 { meat, grain, processed, seafood, fruVeg, condiment }
public enum IngredientType2 { chicken, beef, pork, venison, lamb, rice, wheat, barley, fish, squid, apple, cucumber, salt, pepper, miwon }

public enum CookProcess { none = -1, stirFry, boil, fry, steam, grill, oven }