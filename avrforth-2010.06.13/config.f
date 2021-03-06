( avrforth configuration )

( device must be selected in makefile )

decimal

( cpu clock in hertz )
16000000 constant cpu_frequency

( usart to use for terminal )
01 constant usart

( baud rate of terminal )
38400 constant baud_rate

hex

( usart buffer sizes - must be a power of two )
010 constant usart0_tx_size
010 constant usart0_rx_size
010 constant usart1_tx_size
010 constant usart1_rx_size
  
( interrupt stack sizes )
true constant int_stack
$40 constant int_ds_size
$40 constant int_rs_size

