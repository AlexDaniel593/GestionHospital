ƒ
TC:\Users\dalex\OneDrive\Documentos\Code\GestionHospital\CapaNegocio\TratamientoBL.cs
	namespace

 	
CapaNegocio


 
{ 
public 

class 
TratamientoBL 
{ 
private 
readonly 
TratamientoDAL '
_tratamientoDAL( 7
;7 8
public 
TratamientoBL 
( 
TratamientoDAL +
tratamientoDAL, :
): ;
{ 	
_tratamientoDAL 
= 
tratamientoDAL ,
;, -
} 	
public 
List 
< 
TratamientoViewCLS &
>& '
ListarTratamiento( 9
(9 :
): ;
{ 	
return 
_tratamientoDAL "
." #
ListarTratamiento# 4
(4 5
)5 6
;6 7
} 	
public 
List 
< $
TratamientoMedicoViewCLS ,
>, -$
ListarTratamientosMedico. F
(F G
intG J
idMedicoK S
)S T
{ 	
return 
_tratamientoDAL "
." #$
ListarTratamientosMedico# ;
(; <
idMedico< D
)D E
;E F
} 	
public!! 
TratamientoCLS!! 
?!!  
RecuperarTratamiento!! 3
(!!3 4
int!!4 7
idTratamiento!!8 E
)!!E F
{"" 	
TratamientoCLS## 
tratamientoCLS## )
=##* +
_tratamientoDAL##, ;
.##; < 
RecuperarTratamiento##< P
(##P Q
idTratamiento##Q ^
)##^ _
;##_ `
return$$ 
tratamientoCLS$$ !
;$$! "
}%% 	
public(( 
void(( 
EliminarTratamiento(( '
(((' (
int((( +
idTratamiento((, 9
)((9 :
{)) 	
_tratamientoDAL** 
.** 
EliminarTratamiento** /
(**/ 0
idTratamiento**0 =
)**= >
;**> ?
},, 	
public.. 
void.. 
GuardarTratamiento.. &
(..& '
TratamientoCLS..' 5
tratamiento..6 A
)..A B
{// 	
_tratamientoDAL00 
.00 
GuardarTratamiento00 .
(00. /
tratamiento00/ :
)00: ;
;00; <
}11 	
}44 
}55 óm
QC:\Users\dalex\OneDrive\Documentos\Code\GestionHospital\CapaNegocio\PacienteBL.cs
	namespace 	
CapaNegocio
 
{ 
public 

class 

PacienteBL 
{ 
private		 
readonly		 
PacienteDAL		 $
_pacienteDAL		% 1
;		1 2
private

 
readonly

 
UserManager

 $
<

$ %
IdentityUser

% 1
>

1 2
_userManager

3 ?
;

? @
private 
readonly 
RoleManager $
<$ %
IdentityRole% 1
>1 2
_roleManager3 ?
;? @
private 
readonly  
ApplicationDbContext -
_context. 6
;6 7
public 

PacienteBL 
( 
PacienteDAL %
pacienteDAL& 1
,1 2
UserManager3 >
<> ?
IdentityUser? K
>K L
userManagerM X
,X Y
RoleManagerZ e
<e f
IdentityRolef r
>r s
roleManagert 
,	 Ä
	AccountBL
Å ä
	accountBL
ã î
,
î ï"
ApplicationDbContext
ñ ™
context
´ ≤
)
≤ ≥
{ 	
_pacienteDAL 
= 
pacienteDAL &
;& '
_userManager 
= 
userManager &
;& '
_roleManager 
= 
roleManager &
;& '
_context 
= 
context 
; 
} 	
public 
List 
< 
PacienteCLS 
>  
ListarPaciente! /
(/ 0
)0 1
{ 	
return 
_pacienteDAL 
.  
ListarPaciente  .
(. /
)/ 0
;0 1
} 	
public 
List 
< 
PacienteCLS 
>  $
ListarPacientesAsignados! 9
(9 :
int: =
idDoctor> F
)F G
{ 	
return 
_pacienteDAL 
.  $
ListarPacientesAsignados  8
(8 9
idDoctor9 A
)A B
;B C
} 	
public 
async 
Task 
GuardarPaciente )
() *
PacienteCLS* 5
paciente6 >
)> ?
{   	
_pacienteDAL!! 
.!! 
GuardarPaciente!! (
(!!( )
paciente!!) 1
)!!1 2
;!!2 3
if$$ 
($$ 
paciente$$ 
.$$ 

idPaciente$$ #
==$$$ &
$num$$' (
)$$( )
{%% 
var&& 

contrasena&& 
=&&  
	AccountBL&&! *
.&&* +
GenerarContrasena&&+ <
(&&< =
paciente&&= E
.&&E F
nombre&&F L
,&&L M
paciente&&N V
.&&V W
apellido&&W _
,&&_ `
paciente&&a i
.&&i j
fechaNacimiento&&j y
)&&y z
;&&z {
await))  
CrearUsuarioPaciente)) *
())* +
paciente))+ 3
.))3 4
email))4 9
,))9 :

contrasena)); E
)))E F
;))F G
}** 
}++ 	
public-- 
int-- '
ObtenerIdPacienteDesdeEmail-- .
(--. /
string--/ 5
email--6 ;
)--; <
{.. 	
var// 
doctor// 
=// 
_context// !
.//! "
	PACIENTES//" +
.00 
Where00 
(00 
m00 
=>00  
m00! "
.00" #
email00# (
==00) +
email00, 1
)001 2
.11 
Select11 
(11 
m11 
=>11 !
m11" #
.11# $

idPaciente11$ .
)11. /
.22 
FirstOrDefault22 $
(22$ %
)22% &
;22& '
if44 
(44 
doctor44 
==44 
$num44 
)44 
{55 
throw66 
new66 
	Exception66 #
(66# $
$str66$ `
)66` a
;66a b
}77 
return99 
doctor99 
;99 
}:: 	
public<< 
PacienteCLS<< 
?<< 
RecuperarPaciente<< -
(<<- .
int<<. 1

idPaciente<<2 <
)<<< =
{== 	
return>> 
_pacienteDAL>> 
.>>  
RecuperarPaciente>>  1
(>>1 2

idPaciente>>2 <
)>>< =
;>>= >
}?? 	
publicAA 
asyncAA 
TaskAA 
EliminarPacienteAA *
(AA* +
intAA+ .

idPacienteAA/ 9
)AA9 :
{BB 	
varDD 
pacienteDD 
=DD 
_pacienteDALDD (
.DD( )
RecuperarPacienteDD) :
(DD: ;

idPacienteDD; E
)DDE F
;DDF G
ifEE 
(EE 
pacienteEE 
!=EE 
nullEE  
&&EE! #
pacienteEE$ ,
.EE, -
emailEE- 2
!=EE3 5
nullEE6 :
)EE: ;
{FF 
varHH 
userHH 
=HH 
awaitHH  
_userManagerHH! -
.HH- .
FindByEmailAsyncHH. >
(HH> ?
pacienteHH? G
.HHG H
emailHHH M
)HHM N
;HHN O
ifII 
(II 
userII 
!=II 
nullII  
)II  !
{JJ 
userLL 
.LL 
LockoutEnabledLL '
=LL( )
trueLL* .
;LL. /
userMM 
.MM 

LockoutEndMM #
=MM$ %
DateTimeMM& .
.MM. /
UtcNowMM/ 5
.MM5 6
AddYearsMM6 >
(MM> ?
$numMM? B
)MMB C
;MMC D
varPP 
resultPP 
=PP  
awaitPP! &
_userManagerPP' 3
.PP3 4
UpdateAsyncPP4 ?
(PP? @
userPP@ D
)PPD E
;PPE F
ifQQ 
(QQ 
resultQQ 
.QQ 
	SucceededQQ (
)QQ( )
{RR 
_pacienteDALSS $
.SS$ %
EliminarPacienteSS% 5
(SS5 6

idPacienteSS6 @
)SS@ A
;SSA B
}UU 
elseVV 
{WW 
varXX 
errorsXX "
=XX# $
stringXX% +
.XX+ ,
JoinXX, 0
(XX0 1
$strXX1 5
,XX5 6
resultXX7 =
.XX= >
ErrorsXX> D
.XXD E
SelectXXE K
(XXK L
eXXL M
=>XXN P
eXXQ R
.XXR S
DescriptionXXS ^
)XX^ _
)XX_ `
;XX` a
throwYY 
newYY !
	ExceptionYY" +
(YY+ ,
$strYY, \
+YY] ^
errorsYY_ e
)YYe f
;YYf g
}ZZ 
}[[ 
}\\ 
}__ 	
publicbb 
asyncbb 
Taskbb  
CrearUsuarioPacientebb .
(bb. /
stringbb/ 5
emailbb6 ;
,bb; <
stringbb= C
passwordbbD L
)bbL M
{cc 	
trydd 
{ee 
vargg 
usergg 
=gg 
awaitgg  
_userManagergg! -
.gg- .
FindByEmailAsyncgg. >
(gg> ?
emailgg? D
)ggD E
;ggE F
ifhh 
(hh 
userhh 
==hh 
nullhh  
)hh  !
{ii 
userjj 
=jj 
newjj 
IdentityUserjj +
{kk 
UserNamell  
=ll! "
emailll# (
,ll( )
Emailmm 
=mm 
emailmm  %
}nn 
;nn 
varqq 
resultqq 
=qq  
awaitqq! &
_userManagerqq' 3
.qq3 4
CreateAsyncqq4 ?
(qq? @
userqq@ D
,qqD E
passwordqqF N
)qqN O
;qqO P
ifss 
(ss 
resultss 
.ss 
	Succeededss (
)ss( )
{tt 
awaitvv 
_userManagervv *
.vv* +
AddToRoleAsyncvv+ 9
(vv9 :
uservv: >
,vv> ?
$strvv@ I
)vvI J
;vvJ K
}ww 
elsexx 
{yy 
varzz 
errorszz "
=zz# $
stringzz% +
.zz+ ,
Joinzz, 0
(zz0 1
$strzz1 5
,zz5 6
resultzz7 =
.zz= >
Errorszz> D
.zzD E
SelectzzE K
(zzK L
ezzL M
=>zzN P
ezzQ R
.zzR S
DescriptionzzS ^
)zz^ _
)zz_ `
;zz` a
throw{{ 
new{{ !
	Exception{{" +
({{+ ,
$str{{, I
+{{J K
errors{{L R
){{R S
;{{S T
}|| 
}}} 
}~~ 
catch 
( 
	Exception 
ex 
)  
{
ÄÄ 
throw
ÅÅ 
new
ÅÅ 
	Exception
ÅÅ #
(
ÅÅ# $
$str
ÅÅ$ E
+
ÅÅF G
ex
ÅÅH J
.
ÅÅJ K
Message
ÅÅK R
)
ÅÅR S
;
ÅÅS T
}
ÇÇ 
}
ÉÉ 	
public
ÖÖ 
async
ÖÖ 
Task
ÖÖ 1
#CrearCuentasParaPacientesExistentes
ÖÖ =
(
ÖÖ= >
)
ÖÖ> ?
{
ÜÜ 	
if
àà 
(
àà 
!
àà 
await
àà 
_roleManager
àà #
.
àà# $
RoleExistsAsync
àà$ 3
(
àà3 4
$str
àà4 =
)
àà= >
)
àà> ?
{
ââ 
await
ää 
_roleManager
ää "
.
ää" #
CreateAsync
ää# .
(
ää. /
new
ää/ 2
IdentityRole
ää3 ?
(
ää? @
$str
ää@ I
)
ääI J
)
ääJ K
;
ääK L
}
ãã 
var
éé 
	pacientes
éé 
=
éé 
_pacienteDAL
éé (
.
éé( )
ListarPaciente
éé) 7
(
éé7 8
)
éé8 9
;
éé9 :
foreach
êê 
(
êê 
var
êê 
paciente
êê !
in
êê" $
	pacientes
êê% .
)
êê. /
{
ëë 
if
íí 
(
íí 
paciente
íí 
.
íí 
BHABILITADO
íí (
==
íí) +
$num
íí, -
)
íí- .
{
ìì 
var
ïï 

contrasena
ïï "
=
ïï# $
	AccountBL
ïï% .
.
ïï. /
GenerarContrasena
ïï/ @
(
ïï@ A
paciente
ïïA I
.
ïïI J
nombre
ïïJ P
,
ïïP Q
paciente
ïïR Z
.
ïïZ [
apellido
ïï[ c
,
ïïc d
paciente
ïïe m
.
ïïm n
fechaNacimiento
ïïn }
)
ïï} ~
;
ïï~ 
await
òò "
CrearUsuarioPaciente
òò .
(
òò. /
paciente
òò/ 7
.
òò7 8
email
òò8 =
,
òò= >

contrasena
òò? I
)
òòI J
;
òòJ K
}
ôô 
}
öö 
}
õõ 	
public
ùù 
async
ùù 
Task
ùù 9
+DeshabilitarCuentasDePacientesInhabilitados
ùù E
(
ùùE F
)
ùùF G
{
ûû 	
var
†† 
	pacientes
†† 
=
†† 
_pacienteDAL
†† (
.
††( )"
ListarTodosPacientes
††) =
(
††= >
)
††> ?
;
††? @
foreach
¢¢ 
(
¢¢ 
var
¢¢ 
paciente
¢¢ !
in
¢¢" $
	pacientes
¢¢% .
)
¢¢. /
{
££ 
if
§§ 
(
§§ 
paciente
§§ 
.
§§ 
BHABILITADO
§§ (
==
§§) +
$num
§§, -
)
§§- .
{
•• 
var
ßß 
user
ßß 
=
ßß 
await
ßß $
_userManager
ßß% 1
.
ßß1 2
FindByEmailAsync
ßß2 B
(
ßßB C
paciente
ßßC K
.
ßßK L
email
ßßL Q
)
ßßQ R
;
ßßR S
if
®® 
(
®® 
user
®® 
!=
®® 
null
®®  $
)
®®$ %
{
©© 
user
´´ 
.
´´ 
LockoutEnabled
´´ +
=
´´, -
true
´´. 2
;
´´2 3
user
¨¨ 
.
¨¨ 

LockoutEnd
¨¨ '
=
¨¨( )
DateTime
¨¨* 2
.
¨¨2 3
UtcNow
¨¨3 9
.
¨¨9 :
AddYears
¨¨: B
(
¨¨B C
$num
¨¨C F
)
¨¨F G
;
¨¨G H
var
ØØ 
result
ØØ "
=
ØØ# $
await
ØØ% *
_userManager
ØØ+ 7
.
ØØ7 8
UpdateAsync
ØØ8 C
(
ØØC D
user
ØØD H
)
ØØH I
;
ØØI J
if
∞∞ 
(
∞∞ 
!
∞∞ 
result
∞∞ #
.
∞∞# $
	Succeeded
∞∞$ -
)
∞∞- .
{
±± 
var
≤≤ 
errors
≤≤  &
=
≤≤' (
string
≤≤) /
.
≤≤/ 0
Join
≤≤0 4
(
≤≤4 5
$str
≤≤5 9
,
≤≤9 :
result
≤≤; A
.
≤≤A B
Errors
≤≤B H
.
≤≤H I
Select
≤≤I O
(
≤≤O P
e
≤≤P Q
=>
≤≤R T
e
≤≤U V
.
≤≤V W
Description
≤≤W b
)
≤≤b c
)
≤≤c d
;
≤≤d e
throw
≥≥ !
new
≥≥" %
	Exception
≥≥& /
(
≥≥/ 0
$str
≥≥0 `
+
≥≥a b
errors
≥≥c i
)
≥≥i j
;
≥≥j k
}
¥¥ 
}
µµ 
}
∂∂ 
}
∑∑ 
}
∏∏ 	
}
ππ 
}∫∫ ”I
OC:\Users\dalex\OneDrive\Documentos\Code\GestionHospital\CapaNegocio\MedicoBL.cs
	namespace 	
CapaNegocio
 
{ 
public 

class 
MedicoBL 
{ 
private		 
readonly		 
	MedicoDAL		 "

_medicoDAL		# -
;		- .
private

 
readonly

 
UserManager

 $
<

$ %
IdentityUser

% 1
>

1 2
_userManager

3 ?
;

? @
private 
readonly 
RoleManager $
<$ %
IdentityRole% 1
>1 2
_roleManager3 ?
;? @
private 
readonly  
ApplicationDbContext -
_context. 6
;6 7
public 
MedicoBL 
( 
	MedicoDAL !
	medicoDAL" +
,+ ,
UserManager- 8
<8 9
IdentityUser9 E
>E F
userManagerG R
,R S
RoleManagerT _
<_ `
IdentityRole` l
>l m
roleManagern y
,y z
	AccountBL	{ Ñ
	accountBL
Ö é
,
é è"
ApplicationDbContext
ê §
context
• ¨
)
¨ ≠
{ 	

_medicoDAL 
= 
	medicoDAL "
;" #
_userManager 
= 
userManager &
;& '
_roleManager 
= 
roleManager &
;& '
_context 
= 
context 
; 
} 	
public 
List 
< 
	MedicoCLS 
> 
ListarMedico +
(+ ,
), -
{ 	
return 

_medicoDAL 
. 
ListarMedico *
(* +
)+ ,
;, -
} 	
public 
async 
Task 
GuardarMedico '
(' (
	MedicoCLS( 1
medico2 8
)8 9
{ 	

_medicoDAL 
. 
GuardarMedico $
($ %
medico% +
)+ ,
;, -
if 
( 
medico 
. 
idMedico 
==  "
$num# $
)$ %
{   
var!! 

contrasena!! 
=!!  
	AccountBL!!! *
.!!* +
GenerarContrasena!!+ <
(!!< =
medico!!= C
.!!C D
nombre!!D J
,!!J K
medico!!L R
.!!R S
apellido!!S [
,!![ \
DateTime!!] e
.!!e f
Now!!f i
)!!i j
;!!j k
await## 
CrearUsuarioMedico## (
(##( )
medico##) /
.##/ 0
email##0 5
,##5 6

contrasena##7 A
)##A B
;##B C
}$$ 
}%% 	
public'' 
	MedicoCLS'' 
?'' 
RecuperarMedico'' )
('') *
int''* -
idMedico''. 6
)''6 7
{(( 	
return)) 

_medicoDAL)) 
.)) 
RecuperarMedico)) -
())- .
idMedico)). 6
)))6 7
;))7 8
}** 	
public,, 
async,, 
Task,, 
EliminarMedico,, (
(,,( )
int,,) ,
idMedico,,- 5
),,5 6
{-- 	
var// 
medico// 
=// 

_medicoDAL// #
.//# $
RecuperarMedico//$ 3
(//3 4
idMedico//4 <
)//< =
;//= >
if00 
(00 
medico00 
!=00 
null00 
&&00 !
medico00" (
.00( )
email00) .
!=00/ 1
null002 6
)006 7
{11 
var33 
user33 
=33 
await33  
_userManager33! -
.33- .
FindByEmailAsync33. >
(33> ?
medico33? E
.33E F
email33F K
)33K L
;33L M
if44 
(44 
user44 
!=44 
null44  
)44  !
{55 
user77 
.77 
LockoutEnabled77 '
=77( )
true77* .
;77. /
user88 
.88 

LockoutEnd88 #
=88$ %
DateTime88& .
.88. /
UtcNow88/ 5
.885 6
AddYears886 >
(88> ?
$num88? B
)88B C
;88C D
var;; 
result;; 
=;;  
await;;! &
_userManager;;' 3
.;;3 4
UpdateAsync;;4 ?
(;;? @
user;;@ D
);;D E
;;;E F
if<< 
(<< 
result<< 
.<< 
	Succeeded<< (
)<<( )
{== 

_medicoDAL>> "
.>>" #
EliminarMedico>># 1
(>>1 2
idMedico>>2 :
)>>: ;
;>>; <
}?? 
else@@ 
{AA 
varBB 
errorsBB "
=BB# $
stringBB% +
.BB+ ,
JoinBB, 0
(BB0 1
$strBB1 5
,BB5 6
resultBB7 =
.BB= >
ErrorsBB> D
.BBD E
SelectBBE K
(BBK L
eBBL M
=>BBN P
eBBQ R
.BBR S
DescriptionBBS ^
)BB^ _
)BB_ `
;BB` a
throwCC 
newCC !
	ExceptionCC" +
(CC+ ,
$strCC, Z
+CC[ \
errorsCC] c
)CCc d
;CCd e
}DD 
}EE 
}FF 
}GG 	
publicII 
intII %
ObtenerIdDoctorDesdeEmailII ,
(II, -
stringII- 3
emailII4 9
)II9 :
{JJ 	
varKK 
doctorKK 
=KK 
_contextKK !
.KK! "
MEDICOSKK" )
.LL 
WhereLL 
(LL 
mLL 
=>LL  
mLL! "
.LL" #
emailLL# (
==LL) +
emailLL, 1
)LL1 2
.MM 
SelectMM 
(MM 
mMM 
=>MM !
mMM" #
.MM# $
idMedicoMM$ ,
)MM, -
.NN 
FirstOrDefaultNN $
(NN$ %
)NN% &
;NN& '
ifPP 
(PP 
doctorPP 
==PP 
$numPP 
)PP 
{QQ 
throwRR 
newRR 
	ExceptionRR #
(RR# $
$strRR$ `
)RR` a
;RRa b
}SS 
returnUU 
doctorUU 
;UU 
}VV 	
privateXX 
asyncXX 
TaskXX 
CrearUsuarioMedicoXX -
(XX- .
stringXX. 4
emailXX5 :
,XX: ;
stringXX< B

contrasenaXXC M
)XXM N
{YY 	
varZZ 
userZZ 
=ZZ 
newZZ 
IdentityUserZZ '
{ZZ( )
UserNameZZ* 2
=ZZ3 4
emailZZ5 :
,ZZ: ;
EmailZZ< A
=ZZB C
emailZZD I
}ZZJ K
;ZZK L
var[[ 
result[[ 
=[[ 
await[[ 
_userManager[[ +
.[[+ ,
CreateAsync[[, 7
([[7 8
user[[8 <
,[[< =

contrasena[[> H
)[[H I
;[[I J
if\\ 
(\\ 
result\\ 
.\\ 
	Succeeded\\  
)\\  !
{]] 
await__ 
_userManager__ "
.__" #
AddToRoleAsync__# 1
(__1 2
user__2 6
,__6 7
$str__8 @
)__@ A
;__A B
}`` 
}aa 	
publiccc 
asynccc 
Taskcc -
!CrearCuentasParaMedicosExistentescc ;
(cc; <
)cc< =
{dd 	
ifee 
(ee 
!ee 
awaitee 
_roleManageree #
.ee# $
RoleExistsAsyncee$ 3
(ee3 4
$stree4 <
)ee< =
)ee= >
{ff 
awaitgg 
_roleManagergg "
.gg" #
CreateAsyncgg# .
(gg. /
newgg/ 2
IdentityRolegg3 ?
(gg? @
$strgg@ H
)ggH I
)ggI J
;ggJ K
}hh 
varjj 
medicosjj 
=jj 

_medicoDALjj $
.jj$ %
ListarMedicojj% 1
(jj1 2
)jj2 3
;jj3 4
foreachkk 
(kk 
varkk 
medicokk 
inkk  "
medicoskk# *
)kk* +
{ll 
ifmm 
(mm 
medicomm 
.mm 
BHABILITADOmm &
==mm' )
$nummm* +
)mm+ ,
{nn 
varoo 

contrasenaoo "
=oo# $
	AccountBLoo% .
.oo. /
GenerarContrasenaoo/ @
(oo@ A
medicoooA G
.ooG H
nombreooH N
,ooN O
medicoooP V
.ooV W
apellidoooW _
,oo_ `
DateTimeooa i
.ooi j
Nowooj m
)oom n
;oon o
awaitpp 
CrearUsuarioMedicopp ,
(pp, -
medicopp- 3
.pp3 4
emailpp4 9
,pp9 :

contrasenapp; E
)ppE F
;ppF G
}qq 
}rr 
}ss 	
}vv 
}ww ¡
TC:\Users\dalex\OneDrive\Documentos\Code\GestionHospital\CapaNegocio\FacturacionBL.cs
	namespace 	
CapaNegocio
 
{ 
public 

class 
FacturacionBL 
{ 
private 
readonly 
FacturacionDAL '
_facturacionDAL( 7
;7 8
public 
FacturacionBL 
( 
FacturacionDAL +
facturacionDAL, :
): ;
{ 	
_facturacionDAL 
= 
facturacionDAL ,
;, -
} 	
public 
List 
< 
FacturacionViewCLS &
>& '
ListarFacturacion( 9
(9 :
): ;
{ 	
return 
_facturacionDAL "
." #
ListarFacturacion# 4
(4 5
)5 6
;6 7
} 	
public 
List 
< 
FacturacionCLS "
>" #%
ListarFacturacionPaciente$ =
(= >
int> A

idPacienteB L
)L M
{ 	
return 
_facturacionDAL "
." #%
ListarFacturacionPaciente# <
(< =

idPaciente= G
)G H
;H I
} 	
public 
FacturacionCLS 
?  
RecuperarFacturacion 3
(3 4
int4 7
idFacturacion8 E
)E F
{ 	
FacturacionCLS 
? 
factura #
=$ %
_facturacionDAL& 5
.5 6 
RecuperarFacturacion6 J
(J K
idFacturacionK X
)X Y
;Y Z
return 
factura 
; 
} 	
public   
void   
EliminarFacturacion   '
(  ' (
int  ( +
idFacturacion  , 9
)  9 :
{!! 	
_facturacionDAL"" 
."" 
EliminarFacturacion"" /
(""/ 0
idFacturacion""0 =
)""= >
;""> ?
}$$ 	
public&& 
void&& 
GuardarFacturacion&& &
(&&& '
FacturacionCLS&&' 5
facturacion&&6 A
)&&A B
{'' 	
_facturacionDAL(( 
.(( 
GuardarFacturacion(( .
(((. /
facturacion((/ :
)((: ;
;((; <
})) 	
}** 
}++ ‡
MC:\Users\dalex\OneDrive\Documentos\Code\GestionHospital\CapaNegocio\CitaBL.cs
	namespace 	
CapaNegocio
 
{ 
public 

class 
CitaBL 
{ 
private 
readonly 
CitaDAL  
_citaDAL! )
;) *
public

 
CitaBL

 
(

 
CitaDAL

 
citaDAL

 %
)

% &
{ 	
_citaDAL 
= 
citaDAL 
; 
} 	
public 
List 
< 
CitaViewCLS 
>  

ListarCita! +
(+ ,
), -
{ 	
return 
_citaDAL 
. 

ListarCita &
(& '
)' (
;( )
} 	
public 
List 
< 
CitaViewCLS 
>  
ListarCitasMedico! 2
(2 3
int3 6
idMedico7 ?
)? @
{ 	
return 
_citaDAL 
. 
ListarCitasMedico -
(- .
idMedico. 6
)6 7
;7 8
} 	
public 
List 
< 
CitaViewCLS 
>  
ListarCitasPaciente! 4
(4 5
int5 8

idPaciente9 C
)C D
{ 	
return 
_citaDAL 
. 
ListarCitasPaciente /
(/ 0

idPaciente0 :
): ;
;; <
} 	
public 
void 
GuardarCita 
(  
CitaCLS  '
cita( ,
), -
{   	
_citaDAL!! 
.!! 
GuardarCita!!  
(!!  !
cita!!! %
)!!% &
;!!& '
}"" 	
public$$ 
CitaCLS$$ 
?$$ 
RecuperarCita$$ %
($$% &
int$$& )
idCita$$* 0
)$$0 1
{%% 	
return&& 
_citaDAL&& 
.&& 
RecuperarCita&& )
(&&) *
idCita&&* 0
)&&0 1
;&&1 2
}'' 	
public)) 
void)) 
EliminarCita))  
())  !
int))! $
idCita))% +
)))+ ,
{** 	
_citaDAL++ 
.++ 
EliminarCita++ !
(++! "
idCita++" (
)++( )
;++) *
},, 	
}// 
}00 ˘

PC:\Users\dalex\OneDrive\Documentos\Code\GestionHospital\CapaNegocio\AccountBL.cs
	namespace 	
CapaNegocio
 
{ 
public 

class 
	AccountBL 
{ 
public 
static 
string 
GenerarContrasena .
(. /
string/ 5
nombre6 <
,< =
string> D
apellidoE M
,M N
DateTimeO W
fechaNacimientoX g
)g h
{ 	
var 
primeraLetraNombre "
=# $
nombre% +
.+ ,
	Substring, 5
(5 6
$num6 7
,7 8
$num9 :
): ;
.; <
ToUpper< C
(C D
)D E
;E F
var		 
apellidoMinuscula		 !
=		" #
apellido		$ ,
.		, -
ToLower		- 4
(		4 5
)		5 6
;		6 7
var

 
anoNacimiento

 
=

 
fechaNacimiento

  /
.

/ 0
Year

0 4
.

4 5
ToString

5 =
(

= >
)

> ?
;

? @
return 
primeraLetraNombre %
+& '
apellidoMinuscula( 9
+: ;
anoNacimiento< I
+J K
$strL O
;O P
} 	
} 
} 