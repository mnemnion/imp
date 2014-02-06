
: report-val
	if 
		." good value "
	else 
		." bad value "
	then ;

: simplecase 
	dup

	case 1 of . true endof
	 	 2 of . true endof
	 	 3 of . true endof
	. false 
	endcase
	report-val
	;