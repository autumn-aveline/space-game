import bpy
import math
from math import radians

numBigRoads    = 2
numLocalRoads  = 5
numAlleys      = numLocalRoads - 1

#numBlocksBig   = 1
#numBlocksLocal = 1

blockSizeBig   = 120
blockSizeLocal = 270

roadWidthBig   = 30
roadWidthLocal = 20
roadWidthAlley = 6

def main():
    for i in range(numBigRoads):
        r = RoadSegment(i*blockSizeLocal, 0, blockSizeBig, roadWidthBig)
        r.drawSegment()
        
    for i in range(numLocalRoads):
        r = RoadSegment(0, i*blockSizeBig, blockSizeLocal, roadWidthLocal, 90)
        r.drawSegment()
        
    

def main2():
    a = RoadSegment(0,0,0,blockSizeBig,roadWidthBig)
    b = RoadSegment(0,blockSizeBig,0,blockSizeBig+roadWidthLocal, roadWidthBig)
    c = RoadSegment(0,blockSizeBig+roadWidthLocal,0,blockSizeBig*2+roadWidthLocal,roadWidthBig)
    
    print('/n/n/n')
    print(a)
    print(b)
    print(c)
    
    a.addNext(b)
    b.addNext(c)
    
    print("\n\nb and c")
    print(a.nextSegment)
    print(b.nextSegment)
    
    a.drawSegment()
    a.drawNextSegment()
    #b.drawSegment()
    #c.drawSegment()
    #a.drawConnections()
    #a.connections[0].drawSegment()
    #print(a.connections[0])
    

class Vec2():
    def __init__(self, x, y):
        self.x = x
        self.y = y
    
    @classmethod
    def getDistance(cls, v1, v2):
        return (v2.x-v1.x)^2 + (v2.y-v1.y)^2
    
    def __str__(self):
        return f'x: {self.x} y: {self.y}'



class RoadSegment():
    def __init__(self, x, y, len, width, rotation=0):
        self.x = x
        self.y = y
        self.len = len
        self.width = width
        self.rotation = rotation
    
    def __str__(self):
        return f'ROADSEGMENT\npos: ({self.x}, {self.y}) \nlen: {self.len} \nwidth: {self.width}'
    
    
    def drawSegment(self, name="Generated_mesh"):
        for o in bpy.context.scene.objects:
            if o.name == name:
                bpy.ops.object.select_all(action='DESELECT')
                bpy.data.objects[name].select_set(True)
                #bpy.ops.object.delete()
            
        bpy.ops.mesh.primitive_plane_add()
        so = bpy.context.active_object
        mesh = so.data
        so.location[0] = 0
    
        mesh.vertices[0].co[0] = -self.width/2
        mesh.vertices[0].co[1] = 0
        
        mesh.vertices[1].co[0] = self.width/2
        mesh.vertices[1].co[1] = 0
        
        mesh.vertices[2].co[0] = -self.width/2
        mesh.vertices[2].co[1] = self.len
        
        mesh.vertices[3].co[0] = self.width/2
        mesh.vertices[3].co[1] = self.len
        
        #for v in mesh.vertices:
            #print(v.co)
        
        so.location = (self.x, self.y, 0)
        so.rotation_euler = (0, 0, radians(self.rotation))
        
        so.name = name
        self.isDrawn = True
        


        


main()
