from gtts import gTTS
def greetings(arabic_text, path):
    tts = gTTS(arabic_text, lang='ar')
    tts.save(path)
    return 'Okk'
