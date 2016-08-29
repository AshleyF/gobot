# GoBot

A robot that plays Go!

It is a uArm with OpenCV using GnuGo.

## Installation

  apt-get install gnugo
  apt-get install nuget

  nuget restore
  xbuild gobot.sln TODO

## Running

Start the game engine in GTP mode:

  /usr/games/gnugo --quiet --never-resign --monte-carlo --mode gtp --gtp-listen 1234

TODO
