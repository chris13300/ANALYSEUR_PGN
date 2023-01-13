# ANALYSEUR PGN
Tool to find the best move by consensus of 2 engines

Prerequisites :<br>
copy [detailler_pgn.exe](https://github.com/chris13300/detailler_pgn/releases/download/v1.0.0.0/detailler_pgn.exe) to your ANALYSEUR PGN folder<br>
copy [nettoyer_epd.exe](https://github.com/chris13300/nettoyer_epd/releases/download/v1.0.0.0/nettoyer_epd.exe) to your ANALYSEUR PGN folder<br>
copy [pgn-extract.exe](https://github.com/chris13300/ANALYSEUR_PGN/releases/download/v1.0.0.0/pgn-extract.exe) to your ANALYSEUR PGN folder<p>

rename BUREAU.ini to YOUR_COMPUTER_NAME.ini<br>
set lblChemin1 to path_to_your_engine1<br>
set lblChemin2 to path_to_your_engine2<br>
set cmdOptions1 to path_to_your_text_editor<p>

rename ENGINE1.ini to NAME_OF_YOUR_ENGINE1.ini<br>
set its UCI options<p>

rename ENGINE2.ini to NAME_OF_YOUR_ENGINE2.ini<br>
set its UCI options<p>

# How it works ?
There are different ways to use this tool :<br>
- with an analysis delay, both engines will analyse the last position from each game and store their best moves in a REP file<br>
![pgn_delay](https://github.com/chris13300/ANALYSEUR_PGN/blob/main/ANALYSEUR%20PGN/bin/x64/Debug/pgn_delay.jpg)<br>
![epd_delay](https://github.com/chris13300/ANALYSEUR_PGN/blob/main/ANALYSEUR%20PGN/bin/x64/Debug/epd_delay.jpg)<p>
  
- with a fixed depth, only one engine (blue) will analyse the last position from each game and store their best moves in a REP file<br>
![pgn_depth](https://github.com/chris13300/ANALYSEUR_PGN/blob/main/ANALYSEUR%20PGN/bin/x64/Debug/pgn_depth.jpg)<br>
![epd_depth](https://github.com/chris13300/ANALYSEUR_PGN/blob/main/ANALYSEUR%20PGN/bin/x64/Debug/epd_depth.jpg)<p>

During the analyses, ANALYSEUR PGN captures its window each [15 minutes](https://github.com/chris13300/ANALYSEUR_PGN/blob/main/ANALYSEUR%20PGN/frmPrincipale.vb#L1029).<br>
This allows the user to remotely monitor the progress of the analyses.<p>
  
At the end of the analyses, we get few files :<br>
- the "your_pgn.log" file contains the start time, the end time, the elapsed seconds and the number of analyzed positions<br>
- the "your_pgn.rep" file contains the best moves of each engine for each analyzed position. This file follows the same format as the REP file produced by the Arena's built-in auto-analysis function<br>
- the "pgn/epd - core/threads - delay/depth - hash.log" file contains the elapsed seconds until the last move's change, the percentage of use of the hash, the number of move's changes<br>
- the "engine_name (threads).log" file contains the speed, the depth and the tablebases hits<br>
- the "changes.log" file contains the analyzed positions where there were more than [17 move's changes](https://github.com/chris13300/ANALYSEUR_PGN/blob/main/ANALYSEUR%20PGN/frmPrincipale.vb#L793)<p>

# tips
Use an EPD file when positions contain little material such as end game positions or for legal positions but you don't know what sequence of moves to get there. With the EPD file, the selected depth won't be displayed.<p>

If you double-click on the main window, you can expand its width and see a chessboard on the right.<br>
Double-click the main window again to reduce its width and hide the chessboard.<p>

If you double-click on the 2nd engine path, you can deactivate it.<p>

By default, "Duration" accepts an analysis delay in seconds but it can also accept a fixed depth by adding "D".<br>
For example, enter "30" to search for the best move for 30 seconds or "D30" to search for the best move at depth 30.<br>
You can only use 2 engines with an analysis delay. If you set a fixed depth, only the engine 1 (blue) will be used.<p>

When the "Fixed" option isn't checked, if the 2 engines disagree on the best move, ANALYSEUR PGN can add [60 sec](https://github.com/chris13300/ANALYSEUR_PGN/blob/main/ANALYSEUR%20PGN/frmPrincipale.vb#L9) to the [analysis delay](https://github.com/chris13300/ANALYSEUR_PGN/blob/main/ANALYSEUR%20PGN/frmPrincipale.vb#L1406) in order to allow more time for the engines to agree.<br>
When the "Fixed" option is checked, even if the 2 engines disagree on the best move, ANALYSEUR PGN will respect the analysis delay.<p>

"Delayed start" allows you to schedule the time when the analyses will start.<p>

"Shutdown" will automatically shutdown your computer after the [last analysis](https://github.com/chris13300/ANALYSEUR_PGN/blob/main/ANALYSEUR%20PGN/frmPrincipale.vb#L816).<p>
