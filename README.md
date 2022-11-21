# ANALYSEUR_PGN
Tool to find the best move by consensus of 2 engines

Prerequisites :<br>
copy detailler_pgn.exe to your ANALYSEUR PGN folder<br>
copy nettoyer_epd.exe to your ANALYSEUR PGN folder<br>
copy pgn-extract.exe to your ANALYSEUR PGN folder<p>

rename BUREAU.ini to YOUR_COMPUTER_NAME.ini<br>
set lblChemin1 to path_to_your_engine1<br>
set lblChemin2 to path_to_your_engine2<br>
set cmdOptions1 to path_to_your_text_editor<p>

rename ENGINE1.ini to NAME_OF_YOUR_ENGINE1.ini<br>
set its UCI options<p>

rename ENGINE2.ini to NAME_OF_YOUR_ENGINE2.ini<br>
set its UCI options<p>

# How it works ?

If you double-click on the main window, you can expand its width and see a chessboard on the right.<br>
Double-click the main window again to reduce its width and hide the chessboard.<p>

If you double-click on the 2nd engine path, you can deactivate it.<p>

By default, "Duration" accepts an analysis delay in seconds but it can also accept a fixed depth by adding "D".<br>
For example, enter "30" to search for the best move for 30 seconds or "D30" to search for the best move at depth 30.<br>
You can only use 2 engines with an analysis delay. If you set a fixed depth, only the engine 1 (blue) will be used.<p>

When "Fixed" is not checked, if the 2 engines disagree on the best move, ANALYSEUR PGN can add 60 sec to the analysis delay in order to allow more time for the engines to agree.<br>
When "Fixed" is checked, even if the 2 engines disagree on the best move, ANALYSEUR PGN will respect the analysis delay.<p>

"Delayed start" allows you to schedule the time when the analyses will start.<p>

"Shutdown" will automatically shutdown your computer after the last analysis.<p>
