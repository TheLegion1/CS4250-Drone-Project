import socketserver
import socket
import serial

print("Server Started")

class MyTCPHandler(socketserver.BaseRequestHandler):



    def handle(self):
        self.data = self.request.recv(1024).strip()
        #print("{} wrote:".format(self.client_address[0]))
        #print(self.data)
        parseData(self.data)

    #end class

#start main file functions here

def parseData(data):
    ser = serial.Serial('/dev/ttyACM0', 9600)
    print(data)
    ser.write(data)



#return our string after coverting back to bytes
def ConvertToBytes(string):
    return str.encode(string)

if __name__ == "__main__":
    ser = serial.Serial('/dev/ttyACM0', 9600)
    HOST, PORT = "192.168.0.122", 11000
    server = socketserver.TCPServer((HOST, PORT), MyTCPHandler)
    server.serve_forever()
