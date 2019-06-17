# chatProgram
C#

채팅 프로그램 이름 관리
json 컨트롤러에서 처리
dataPacket 에 보낼 데이터들 명확하게 정립
다 끝나고 #region 처리

Work
	login - login_re
	register - register_re
	add_friend - add_friend_re
	friend_list - friend_list_re
	del_friend - del_friend_re
	new_chat - new_chat_re
	invite_friend - invite_friend_re
	chat_out - chat_out_re
	chat_room_out - chat_room_out_re
	chat_in - chat_in_re
	send_message - send_message_re
	user_update - user_update_re
	

프로그램 실행 시 소켓 연결, 종료 시 소켓 연결 끊음

채팅 클라이언트 실행시 소켓 연결되는데까지 함

딕셔너리에 클라이언트 묶어서 넣는 부분을 로그인 할때 처리하도록 바꾸는 것이 좋아보임

클라이언트 소켓이 어떤 유저인지 구분하는 방법 필요  - 딕셔너리
로그인 시 입력한 아이디랑 소켓 묶어서 두고
메시지 보냈을 때 그 채팅방에 속한 인원들이 딕셔너리에 소켓 열렸는지(있는지) 확인한다음
열려있는 애들한테만 보낸다.

1. 각 페이지들 구성
2. DB설계 및 연동
3. DB에 맞춰서 데이터 패킷 조정

클라이언트 각 폼에서는 전역변수 client로 스트림 사용
쓰레드 사용 중 폼 이동할 때는 Abort로 종료 후 넘기기
각 폼마다 어플리케이션 종료 이벤트 추가

