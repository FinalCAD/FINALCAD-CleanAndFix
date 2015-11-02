# FINALCAD-CleanAndFix
AutoCAD plugin with usefull commands to clean and fix plans.

FINALCAD provides free of charge and as open source software, the “Clean & Fix” plugin, allowing the cleaning and readability enhancements for any kind of AutoCAD blueprint.
AutoCAD plugin can be found here: https://apps.autodesk.com/ACD/fr/Detail/Index?id=appstore.exchange.autodesk.com%3afinalcadcleanfix_windows32and64%3aen

## General Usage Instructions

FINALCAD Clean & Fix will allow a user to work with from an external drawing in the quickest way.
If the user need to do some modification on the plan, the FCCLEAN command will allow to erase all potential errors.
DARKER, BLACK, GRAYSCALE and ZEROOPACITY is usefull if you need to have a bette view before a ploting on paper, and working with a white background.

## Commands

| Ribbon/Toolbar Icon | Command | Command Description |
|---|---|---|
|  | FCCLEAN |  FcClean will execute different procedures to correct the plan and make it easier editiable / usable: Reconcile & unlock all layers, Erase proxies, Clean Entities  |
|  | FCDYNTOSTATIC | FcDynToStatic will modify all dynamic entities to a static one.|
|  | FCBLACK | FcBlack will change all drawing colors to black |
|  | FCDARKER | FcDarker will change all drawing colors to darker (30% max luminosity) |
|  | FCGRAYSCALE | FcGrayscale will change all drawing colors to grayscale |
|  | FCZEROOPACITY | FcZeroOpacity will remove all transparencies in the drawing. |
|  | FCFILTER | FcFilter turn off all layer contain the specified keword define by the user. |
|  | FCRFILTER | FcRFilter turn on all layer contain the specified keword define by the user. |
|  | FCMERGETEXT | FcMergeText will merge all selected texts to a new one. |
