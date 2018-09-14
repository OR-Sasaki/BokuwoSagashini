from multiprocessing import Pool
import copy
#################
path='/Users/hemuichi/Desktop/develop/python/Text/SumpleText.txt'
#################
H=3
V=3
C=3
Ko=3

## ClearFile
def CF():
    f=open(path,'w')
    
## WriteFile
def OPF(x):
    f=open(path,'a')
    f.write(x)

## ViewFile
def VF():
    for x in open(path,'r'):
        print(x)

## CombertAlfabet
def CombertAlf(x):
    asc = 'ABCDEFGHIJKLMNOPQRSTUVWXYZ'
    return asc[x]
    
## ClearCheck
def ClearCheck(b):
    arr= [[1,0,1,1],[0,1,1,-1]]
    for i in range(H):
        for k in range(V):
            if(b[i][k]>=0):
                env=[[0 for hoge1 in range(Ko)] for k in range(4)]
                for cle in range(C-1):
                    for ar in range(4):
                        if((((i+arr[0][ar]*(cle+1))<=(H-1))and((k+arr[1][ar]*(cle+1))<=(V-1)))\
                           and((i+arr[0][ar]*(cle+1))>=0)and((k+arr[1][ar]*(cle+1))>=0)):
                            for bc in range(Ko):
                                bmask=2**bc
                                if(( (b[i][k]&bmask) == (b[i+arr[0][ar]*(cle+1)][k+arr[1][ar]*(cle+1)]&bmask) )and\
                                   (b[i+arr[0][ar]*(cle+1)][k+arr[1][ar]*(cle+1)])>=0):

                                    #print(str(b[i][k])+':'+str(b[i+arr[0][ar]*(cle+1)][k+arr[1][ar]*(cle+1)])+':'+bin(bmask))

                                    env[ar][bc]+=1
                                    if(env[ar][bc]==(C-1)):
                                        return 1
    return 0

## Dicision
def Dicision(board,koma=0,Dup=[],turn=1,x='',tc=1):
    cp=ClearCheck(board)
    if(cp==0):
        able=0
        alc=1;
        try:
            for i in range(H):
                for k in range(V):
                    if(board[i][k]==-1):
                        able=1
                        Winner=0
                        
                        BoardBuf=copy.deepcopy(board)
                        BoardBuf[i][k]=koma
                        
                        DupBuf=copy.deepcopy(Dup)
                        DupBuf.append(koma)
                        
                        for m in range(2**Ko):
                            if((m not in DupBuf )\
                               or ((m==0) and (len(DupBuf)==2**Ko)) ):
                                
                                Winner=Dicision( BoardBuf,\
                                                 m,\
                                                 DupBuf,\
                                                 2 if turn==1 else 1,\
                                                 str(x) + '['+ str(koma)+ ':' + CombertAlf(i) + ',' + str(k) + ']',\
                                                 tc+1)
                        
                                if(turn==1):
                                    if(Winner==1):
                                        alc*=0
                                        raise Exception
                                    else:
                                        pass
                                elif(turn==2):
                                    if(Winner==1):
                                        alc*=1
                                    else:
                                        alc*=0
                                        raise Exception
        except Exception:
            pass
        
        if(able==0):
            pass
            #OPF(str(x)+':Draw\n')
        else:
            if(turn==1):
                if(alc==0):
                    return 1
                else:
                    return 0
            elif(turn==2):
                if(alc==1):
                    if(tc==2):
                        print(x+':'+str(koma))
                    return 1
                else:
                    return 0
        
    else:
        cp=2 if turn==1 else 1
        #OPF(str(x)+':Player'+str(cp)+'\n')
        return cp

## MultiFunction
def f(fkoma):
    
    fDup=[0,12,4,10,11,13,3,9]
    ccc=1
    
    for fi in range(H):
            for fk in range(V):
                if(Board[fi][fk]==-1):
                    
                    fBoardBuf=copy.deepcopy(Board)
                    fBoardBuf[fi][fk]=fkoma
                    
                    fDupBuf=copy.deepcopy(fDup)
                    fDupBuf.append(fkoma)
                    
                    for fm in range(2**Ko):
                        if(fm not in fDupBuf ):
                            
                            ccc*=Dicision( fBoardBuf,\
                                             fm,\
                                             fDupBuf,\
                                             1,\
                                             '['+ str(fkoma)+ ':' + CombertAlf(fi) + ',' + str(fk) + ']',\
                                             1)  ##tc
    if(ccc==1):
        return 1
    else:
        return 0               

## MainFunction
if __name__ == '__main__':
    print('start')
    Board=[[-1 for i in range(V)] for k in range(H)]
    #CF()
    
    Board=[[-1,-1,13,-1],\
           [-1, 0,12,10],\
           [-1,11, 4,-1],\
           [-1, 9, 3,-1]]
    
    with Pool(4) as p:
        print(p.map(f,[1,5,2,15,8,14,6,7]))




