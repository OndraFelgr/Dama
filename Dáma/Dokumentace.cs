/*
proměnné:
-board: herní pole o velikosti 8x8, tvořené tlačítky
-isWhiteTurn: určuje, zda je na tahu bílý hráč
-selectedButton: ukládá figurku, která je právě vybraná k tahu
Funkce a logika hry:
InitializeBoard
Vytváří mřížku 8x8, přiřazuje barvy polí a rozmísťuje bílé a černé figurky na pozice
Piece_Click
Reaguje na kliknutí hráče:
-klikne na svou figurku tak ji označí 
-klikne se na jiné pole tak se pokusí provést tah
-Pokud tah zahrnuje přeskok přes soupeřovu figurku, ta je odstraněna.
-Po přeskoku se kontroluje, zda lze provést další skok. Pokud ano, hráč může pokračovat.
IsValidMove
Ověřuje platnost tahu. Povolen je:
-o jedno pole diagonálně vpřed.
-preskok o dvě pole přes soupeřovu figurku.
CanJumpAgain
Zjišťuje, zda po provedeném skoku může hráč pokračovat dalším přeskokem z nové pozice.
*/