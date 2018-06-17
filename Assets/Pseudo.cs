/* INPUT UPDATE PSEUDO-CODE
when input string changes (add char, add space, remove char, remove space, remove range, add range)
        
get how many parts input can be split into

if first part is different from previous input
    update command input
        
if there is a command selected:
_______________________________________________
if command variables don't match arguments
        rebuild arguments

if input has any parts
	let argInput = input without first index

while argInput has parts
    for every argument
        for however many inputs the argument needs
            add first argInput
            remove first argInput
_______________________________________________
        
*/