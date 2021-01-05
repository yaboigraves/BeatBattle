# read the contents of a midi file, extracts the time of midi events for kick hat snare. convert those times into y positions
# where each 1 unit is 1 beat


# needs to convert time -> beat
# beat = time(seconds) * (bpm/60)

from mido import MidiFile
import mido
#import midi
import sys


class MidiParser:

    def parse(self,filePath,bpmArg):
        totTime = 0

        # midi file
        # mid = MidiFile('midis/sonic midi shitfuck.mid')
        midiFilePath = filePath
        mid = MidiFile(midiFilePath)

        # bpm
        bpm = float(bpmArg)

        # times of midi events in millis for each track
        kickMessages = []
        hatMessages = []
        snareMessages = []

        # 96 bpm
        # 4 beats per bar
        # 24 bars per minute
        # 60/24 = 2.5 bars
        # each midi is 2 bars

        # gotta figure out how to calculate this based on bpm


        for i, track in enumerate(mid.tracks):
            totTime -= track[3].time
            # for msg in track:
            for j in range(3, len(track) - 1):
                # print(track[j].type)
                totTime += track[j].time
                if(track[j].type == "note_on"):
                    if(track[j].note == 36):
                        kickMessages.append(mido.tick2second(
                            totTime, mid.ticks_per_beat, mido.bpm2tempo(bpm)) * (bpm/60))
                    if(track[j].note == 38):
                        snareMessages.append(mido.tick2second(
                            totTime, mid.ticks_per_beat, mido.bpm2tempo(bpm)) * (bpm/60))

        # note this gives the time of the last note off event, not the actual end of the midi track
        #print(mido.tick2second(totTime, mid.ticks_per_beat, mido.bpm2tempo(bpm)) * 2)
        print(kickMessages)
        print(snareMessages)

        kM = ""

        for kickMessage in kickMessages:
            kM += str(kickMessage) + " "
        

        return kM


        

  


        #no longer need to write to a file
        # f = open("sucess.txt", "w")

        # f.write("k " + str(kickMessages) + "\n")
        # f.write("s " + str(snareMessages) + "\n")
        # f.write("l " + str(mido.tick2second(totTime,
        #                                     mid.ticks_per_beat, mido.bpm2tempo(bpm) * (bpm/60))) + "\n")
        # # f.write("t " + str(secondsPer2Bars) + "\n")

        # f.close()
