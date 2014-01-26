( mcp2515 )
( daniel j kruszyna )
( http://krue.net/avr/ )
( 2006.06.19 )

label can_init
  mcp2515 clear-bit
  wl 02 load spi rcall,
  wl 28 load spi rcall,
  wl 01 load spi rcall, ( cnf3 ph2=2Tq )
  wl 8a load spi rcall, ( cnf2 ph1=2Tq pr=3Tq )
  wl 00 load spi rcall, ( cnf1 sjw=1Tq brp=/2 )
  wl 03 load spi rcall, ( caninte rx1ie,rx0ie )
  mcp2515 set-bit
  ret,

label can_reset
  mcp2515 clear-bit
  wl c0 load
  spi rcall,
  mcp2515 set-bit

  ( mcp2515 does not respond for a while after reset )
  wait rcall,
  ret,

label can_status
  mcp2515 clear-bit
  wl a0 load
  spi rcall,
  spi rcall,
  mcp2515 set-bit
  ret,

label can_rxstatus
  mcp2515 clear-bit
  wl b0 load
  spi rcall,
  spi rcall,
  mcp2515 set-bit
  ret,

label can_rx0
  mcp2515 clear-bit
  wl 92 load
  spi rcall,
  ret,

label can_rx1
  mcp2515 clear-bit
  wl 96 load
  spi rcall,
  ret,

00 constant rx_filter0
04 constant rx_filter1
08 constant rx_filter2
10 constant rx_filter3
14 constant rx_filter4
18 constant rx_filter5
20 constant rx_mask0
24 constant rx_mask1

label can_setid
  xl lsl, xh rol,
  xl lsl, xh rol,
  xl lsl, xh rol,
  xl lsl, xh rol,
  xl lsl, xh rol,

  mcp2515 clear-bit
  wl 02 load
  spi rcall,

  wl wh mov,
  spi rcall,

  wl xh mov,
  spi rcall,
  wl xl mov,
  spi rcall,
  wl 00 load
  spi rcall,
  wl 00 load
  spi rcall,
  mcp2515 set-bit
  ret,

label can_bitmodify
  mcp2515 clear-bit
  wl 05 load
  spi rcall,
  wl wh mov,
  spi rcall,
  wl xl mov,
  spi rcall,
  wl xh mov,
  spi rcall,
  mcp2515 set-bit
  ret,

label can_config
  wh 0f load
  xl e0 load
  xh 80 load
  can_bitmodify rjmp,

label can_normal
  wh 0f load
  xl e0 load
  xh 00 load
  can_bitmodify rjmp,

label can_enable_output
  wh 0c load
  xl 3f load
  xh 0c load
  can_bitmodify rjmp,

label can_output0_on
  wh 0c load
  xl 10 load
  xh 10 load
  can_bitmodify rjmp,

label can_output0_off
  wh 0c load
  xl 10 load
  xh 00 load
  can_bitmodify rjmp,

label can_output1_on
  wh 0c load
  xl 20 load
  xh 20 load
  can_bitmodify rjmp,

label can_output1_off
  wh 0c load
  xl 20 load
  xh 00 load
  can_bitmodify rjmp,

