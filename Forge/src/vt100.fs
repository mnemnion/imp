\ VT100.STR     VT100 excape sequences                  20may93jaw

\ Copyright (C) 1995,1999,2000,2003,2007 Free Software Foundation, Inc.

\ This file is part of Gforth.

\ Gforth is free software; you can redistribute it and/or
\ modify it under the terms of the GNU General Public License
\ as published by the Free Software Foundation, either version 3
\ of the License, or (at your option) any later version.

\ This program is distributed in the hope that it will be useful,
\ but WITHOUT ANY WARRANTY; without even the implied warranty of
\ MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
\ GNU General Public License for more details.

\ You should have received a copy of the GNU General Public License
\ along with this program. If not, see http://www.gnu.org/licenses/.

decimal

: pn    base @ swap decimal 0 u.r base ! ;
: ;pn   [char] ; emit pn ;
: ESC[  27 emit [char] [ emit ;

: at-xy ( u1 u2 -- ) \ facility at-x-y
  \ Position the cursor so that subsequent text output will take
  \ place at column @var{u1}, row @var{u2} of the display. (column 0,
  \ row 0 is the top left-hand corner of the display).
  1+ swap 1+ swap ESC[ pn ;pn [char] H emit ;

: page ( -- ) \ facility
  \ Clear the display and set the cursor to the top left-hand
  \ corner.
  ESC[ ." 2J" 0 0 at-xy ;

