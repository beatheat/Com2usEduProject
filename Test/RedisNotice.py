import redis

r = redis.Redis(host = 'localhost', port =6379, db=0)

noticeString = "\
---공지---\n\
게임첫 발매로 5000코인을 드립니다.\n\
메일함에서 수령해주세요.\n\
"

r.set('Notice', noticeString) 