from mido import MidiFile
import mido
import midi

totTime = 0
bpm = 92
mid = MidiFile('midis/sonic midi shitfuck.mid')

kickMessages = []
hatMessages = []
snareMessages = []

# 96 bpm
# 4 beats per bar
# 24 bars per minute
# 60/24 = 2.5 bars
# each midi is 2 bars

# gotta figure out how to calculate this based on bpm
secondsPer2Bars = (60 / (bpm / 4)) * 2
print(secondsPer2Bars)

for i, track in enumerate(mid.tracks):
    totTime -= track[3].time
    # for msg in track:
    for j in range(3, len(track) - 1):
        # print(track[j].type)
        totTime += track[j].time
        if(track[j].type == "note_off"):
            if(track[j].note == 36):
                kickMessages.append(mido.tick2second(
                    totTime, mid.ticks_per_beat, mido.bpm2tempo(92)))
            if(track[j].note == 38):
                snareMessages.append(mido.tick2second(
                    totTime, mid.ticks_per_beat, mido.bpm2tempo(92)))

# note this gives the time of the last note off event, not the actual end of the midi track
print(mido.tick2second(totTime, mid.ticks_per_beat, mido.bpm2tempo(92)))
print(kickMessages)
print(snareMessages)


f = open("beat3.txt", "w")

f.write("k " + str(kickMessages) + "\n")
f.write("s " + str(snareMessages) + "\n")
f.write("l " + str(mido.tick2second(totTime,
                                    mid.ticks_per_beat, mido.bpm2tempo(92))) + "\n")
f.write("t " + str(secondsPer2Bars) + "\n")

f.close()
