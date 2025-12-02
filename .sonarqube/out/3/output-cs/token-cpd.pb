‡K
RC:\Users\dalex\OneDrive\Documentos\Code\GestionHospital\GestionHospital\Program.cs
var 
builder 
= 
WebApplication 
. 
CreateBuilder *
(* +
args+ /
)/ 0
;0 1
var 
connectionString 
= 
builder 
. 
Configuration ,
., -
GetConnectionString- @
(@ A
$strA E
)E F
;F G
builder		 
.		 
Services		 
.		 
AddDbContext		 
<		  
ApplicationDbContext		 2
>		2 3
(		3 4
options		4 ;
=>		< >
options

 
.

 
UseSqlServer

 
(

 
connectionString

 )
)

) *
)

* +
;

+ ,
builder 
. 
Services 
. 
AddIdentity 
< 
IdentityUser )
,) *
IdentityRole+ 7
>7 8
(8 9
options9 @
=>A C
{ 
options 
. 
Password 
. 
RequireDigit !
=" #
true$ (
;( )
options 
. 
Password 
. 
RequireLowercase %
=& '
true( ,
;, -
options 
. 
Password 
. 
RequireUppercase %
=& '
true( ,
;, -
options 
. 
Password 
. "
RequireNonAlphanumeric +
=, -
true. 2
;2 3
options 
. 
Password 
. 
RequiredLength #
=$ %
$num& '
;' (
options 
. 
SignIn 
. #
RequireConfirmedAccount *
=+ ,
false- 2
;2 3
} 
) 
. $
AddEntityFrameworkStores 
<  
ApplicationDbContext .
>. /
(/ 0
)0 1
. $
AddDefaultTokenProviders 
( 
) 
; 
builder 
. 
Services 
. &
ConfigureApplicationCookie +
(+ ,
options, 3
=>4 6
{ 
options 
. 
	LoginPath 
= 
$str (
;( )
options 
. 
AccessDeniedPath 
= 
$str 6
;6 7
} 
) 
; 
builder   
.   
Services   
.   #
AddControllersWithViews   (
(  ( )
)  ) *
;  * +
builder!! 
.!! 
Services!! 
.!! 
AddRazorPages!! 
(!! 
)!!  
;!!  !
builder$$ 
.$$ 
Services$$ 
.$$ 
	AddScoped$$ 
<$$ 
PacienteDAL$$ &
>$$& '
($$' (
)$$( )
;$$) *
builder%% 
.%% 
Services%% 
.%% 
	AddScoped%% 
<%% 

PacienteBL%% %
>%%% &
(%%& '
)%%' (
;%%( )
builder&& 
.&& 
Services&& 
.&& 
	AddScoped&& 
<&& 
	AccountBL&& $
>&&$ %
(&&% &
)&&& '
;&&' (
builder'' 
.'' 
Services'' 
.'' 
	AddScoped'' 
<'' 
	MedicoDAL'' $
>''$ %
(''% &
)''& '
;''' (
builder(( 
.(( 
Services(( 
.(( 
	AddScoped(( 
<(( 
MedicoBL(( #
>((# $
((($ %
)((% &
;((& '
builder)) 
.)) 
Services)) 
.)) 
	AddScoped)) 
<)) 
CitaDAL)) "
>))" #
())# $
)))$ %
;))% &
builder** 
.** 
Services** 
.** 
	AddScoped** 
<** 
CitaBL** !
>**! "
(**" #
)**# $
;**$ %
builder++ 
.++ 
Services++ 
.++ 
	AddScoped++ 
<++ 
TratamientoDAL++ )
>++) *
(++* +
)+++ ,
;++, -
builder,, 
.,, 
Services,, 
.,, 
	AddScoped,, 
<,, 
TratamientoBL,, (
>,,( )
(,,) *
),,* +
;,,+ ,
builder-- 
.-- 
Services-- 
.-- 
	AddScoped-- 
<-- 
FacturacionDAL-- )
>--) *
(--* +
)--+ ,
;--, -
builder.. 
... 
Services.. 
... 
	AddScoped.. 
<.. 
FacturacionBL.. (
>..( )
(..) *
)..* +
;..+ ,
var11 
app11 
=11 	
builder11
 
.11 
Build11 
(11 
)11 
;11 
if44 
(44 
!44 
app44 
.44 	
Environment44	 
.44 
IsDevelopment44 "
(44" #
)44# $
)44$ %
{55 
app66 
.66 
UseExceptionHandler66 
(66 
$str66 )
)66) *
;66* +
app88 
.88 
UseHsts88 
(88 
)88 
;88 
}99 
app;; 
.;; 
UseHttpsRedirection;; 
(;; 
);; 
;;; 
app<< 
.<< 

UseRouting<< 
(<< 
)<< 
;<< 
app== 
.== 
UseAuthentication== 
(== 
)== 
;== 
app>> 
.>> 
UseAuthorization>> 
(>> 
)>> 
;>> 
appAA 
.AA 
MapStaticAssetsAA 
(AA 
)AA 
;AA 
appBB 
.BB 
MapControllerRouteBB 
(BB 
nameCC 
:CC 	
$strCC
 
,CC 
patternDD 
:DD 
$strDD 5
)DD5 6
.EE 
WithStaticAssetsEE 
(EE 
)EE 
;EE 
usingGG 
(GG 
varGG 

scopeGG 
=GG 
appGG 
.GG 
ServicesGG 
.GG  
CreateScopeGG  +
(GG+ ,
)GG, -
)GG- .
{HH 
varII 
roleManagerII 
=II 
scopeII 
.II 
ServiceProviderII +
.II+ ,
GetRequiredServiceII, >
<II> ?
RoleManagerII? J
<IIJ K
IdentityRoleIIK W
>IIW X
>IIX Y
(IIY Z
)IIZ [
;II[ \
varKK 
rolesKK 
=KK 
newKK 
[KK 
]KK 
{KK 
$strKK 
,KK  
$strKK! )
,KK) *
$strKK+ 4
,KK4 5
$strKK6 =
}KK= >
;KK> ?
foreachLL 
(LL 
varLL 
roleLL 
inLL 
rolesLL 
)LL 
{MM 
ifNN 

(NN 
!NN 
awaitNN 
roleManagerNN 
.NN 
RoleExistsAsyncNN .
(NN. /
roleNN/ 3
)NN3 4
)NN4 5
awaitOO 
roleManagerOO 
.OO 
CreateAsyncOO )
(OO) *
newOO* -
IdentityRoleOO. :
(OO: ;
roleOO; ?
)OO? @
)OO@ A
;OOA B
}PP 
}QQ 
usingSS 
(SS 
varSS 

scopeSS 
=SS 
appSS 
.SS 
ServicesSS 
.SS  
CreateScopeSS  +
(SS+ ,
)SS, -
)SS- .
{TT 
varUU 
userManagerUU 
=UU 
scopeUU 
.UU 
ServiceProviderUU +
.UU+ ,
GetRequiredServiceUU, >
<UU> ?
UserManagerUU? J
<UUJ K
IdentityUserUUK W
>UUW X
>UUX Y
(UUY Z
)UUZ [
;UU[ \
stringVV 

emailVV 
=VV 
$strVV $
;VV$ %
stringWW 

passwordWW 
=WW 
$strWW "
;WW" #
ifXX 
(XX 
awaitXX 
userManagerXX 
.XX 
FindByEmailAsyncXX *
(XX* +
emailXX+ 0
)XX0 1
==XX2 4
nullXX5 9
)XX9 :
{YY 
varZZ 
userZZ 
=ZZ 
newZZ 
IdentityUserZZ #
{ZZ$ %
UserNameZZ& .
=ZZ/ 0
emailZZ1 6
,ZZ6 7
EmailZZ8 =
=ZZ> ?
emailZZ@ E
}ZZF G
;ZZG H
await[[ 
userManager[[ 
.[[ 
CreateAsync[[ %
([[% &
user[[& *
,[[* +
password[[, 4
)[[4 5
;[[5 6
await\\ 
userManager\\ 
.\\ 
AddToRoleAsync\\ (
(\\( )
user\\) -
,\\- .
$str\\/ 6
)\\6 7
;\\7 8
}^^ 
}__ 
app`` 
.`` 
Run`` 
(`` 
)`` 	
;``	 
Ï
cC:\Users\dalex\OneDrive\Documentos\Code\GestionHospital\GestionHospital\Models\RegisterViewModel.cs
	namespace 	
GestionHospital
 
. 
Models  
{ 
public 

class 
RegisterViewModel "
{ 
[ 	
Required	 
] 
[		 	
EmailAddress			 
]		 
[

 	
Display

	 
(

 
Name

 
=

 
$str

 ,
)

, -
]

- .
public 
string 
Email 
{ 
get !
;! "
set# &
;& '
}( )
[ 	
Required	 
] 
[ 	
DataType	 
( 
DataType 
. 
Password #
)# $
]$ %
[ 	
Display	 
( 
Name 
= 
$str $
)$ %
]% &
public 
string 
Password 
{  
get! $
;$ %
set& )
;) *
}+ ,
[ 	
Required	 
] 
[ 	
DataType	 
( 
DataType 
. 
Password #
)# $
]$ %
[ 	
Display	 
( 
Name 
= 
$str .
). /
]/ 0
[ 	
Compare	 
( 
$str 
, 
ErrorMessage )
=* +
$str, l
)l m
]m n
public 
string 
ConfirmPassword %
{& '
get( +
;+ ,
set- 0
;0 1
}2 3
[ 	
Required	 
] 
[ 	
Display	 
( 
Name 
= 
$str  
)  !
]! "
public 
string 
Nombre 
{ 
get "
;" #
set$ '
;' (
}) *
[ 	
Required	 
] 
[ 	
Display	 
( 
Name 
= 
$str "
)" #
]# $
public 
string 
Apellido 
{  
get! $
;$ %
set& )
;) *
}+ ,
[   	
Required  	 
]   
[!! 	
Display!!	 
(!! 
Name!! 
=!! 
$str!! -
)!!- .
]!!. /
["" 	
DataType""	 
("" 
DataType"" 
."" 
Date"" 
)""  
]""  !
public## 
DateTime## 
FechaNacimiento## '
{##( )
get##* -
;##- .
set##/ 2
;##2 3
}##4 5
[%% 	
Required%%	 
]%% 
[&& 	
Display&&	 
(&& 
Name&& 
=&& 
$str&& "
)&&" #
]&&# $
public'' 
string'' 
Telefono'' 
{''  
get''! $
;''$ %
set''& )
;'') *
}''+ ,
[)) 	
Required))	 
])) 
[** 	
Display**	 
(** 
Name** 
=** 
$str** #
)**# $
]**$ %
public++ 
string++ 
	Direccion++ 
{++  !
get++" %
;++% &
set++' *
;++* +
}++, -
},, 
}-- ¥	
`C:\Users\dalex\OneDrive\Documentos\Code\GestionHospital\GestionHospital\Models\LoginViewModel.cs
	namespace 	
GestionHospital
 
. 
Models  
{ 
public 

class 
LoginViewModel 
{ 
[ 	
Required	 
] 
[ 	
EmailAddress	 
] 
public		 
string		 
Email		 
{		 
get		 !
;		! "
set		# &
;		& '
}		( )
[ 	
Required	 
] 
[ 	
DataType	 
( 
DataType 
. 
Password #
)# $
]$ %
public 
string 
Password 
{  
get! $
;$ %
set& )
;) *
}+ ,
[ 	
Display	 
( 
Name 
= 
$str &
)& '
]' (
public 
bool 

RememberMe 
{  
get! $
;$ %
set& )
;) *
}+ ,
} 
} ¿
`C:\Users\dalex\OneDrive\Documentos\Code\GestionHospital\GestionHospital\Models\ErrorViewModel.cs
	namespace 	
GestionHospital
 
. 
Models  
{ 
public 

class 
ErrorViewModel 
{ 
public 
string 
? 
	RequestId  
{! "
get# &
;& '
set( +
;+ ,
}- .
public 
bool 
ShowRequestId !
=>" $
!% &
string& ,
., -
IsNullOrEmpty- :
(: ;
	RequestId; D
)D E
;E F
} 
}		 Ÿ 
lC:\Users\dalex\OneDrive\Documentos\Code\GestionHospital\GestionHospital\Controllers\TratamientoController.cs
	namespace 	
GestionHospital
 
. 
Controllers %
{ 
[		 
	Authorize		 
]		 
public

 

class

 !
TratamientoController

 &
:

' (

Controller

) 3
{ 
private 
readonly 
TratamientoBL &
_tratamientoBL' 5
;5 6
private 
readonly 
MedicoBL !
	_medicoBL" +
;+ ,
public !
TratamientoController $
($ %
TratamientoBL% 2
tratamientoBL3 @
,@ A
MedicoBLB J
medicoBLK S
)S T
{ 	
_tratamientoBL 
= 
tratamientoBL *
;* +
	_medicoBL 
= 
medicoBL  
;  !
} 	
[ 	
	Authorize	 
( 
Roles 
= 
$str 1
)1 2
]2 3
public 
IActionResult 
Index "
(" #
)# $
{ 	
return 
View 
( 
) 
; 
} 	
[ 	
	Authorize	 
( 
Roles 
= 
$str )
)) *
]* +
public 
List 
< 
TratamientoViewCLS &
>& '
ListarTratamiento( 9
(9 :
): ;
{ 	
return   
_tratamientoBL   !
.  ! "
ListarTratamiento  " 3
(  3 4
)  4 5
;  5 6
}"" 	
[$$ 	
	Authorize$$	 
($$ 
Roles$$ 
=$$ 
$str$$ *
)$$* +
]$$+ ,
public%% 
List%% 
<%% $
TratamientoMedicoViewCLS%% ,
>%%, -#
ListarTratamientoMedico%%. E
(%%E F
)%%F G
{&& 	
var'' 
	userEmail'' 
='' 
User''  
.''  !
FindFirstValue''! /
(''/ 0

ClaimTypes''0 :
.'': ;
Email''; @
)''@ A
;''A B
var(( 
idMedico(( 
=(( 
	_medicoBL(( $
.(($ %%
ObtenerIdDoctorDesdeEmail((% >
(((> ?
	userEmail((? H
)((H I
;((I J
return)) 
_tratamientoBL)) !
.))! "$
ListarTratamientosMedico))" :
()): ;
idMedico)); C
)))C D
;))D E
}++ 	
[.. 	
	Authorize..	 
(.. 
Roles.. 
=.. 
$str.. 1
)..1 2
]..2 3
public// 
TratamientoCLS// 
?//  
RecuperarTratamiento// 3
(//3 4
int//4 7
idTratamiento//8 E
)//E F
{00 	
return11 
_tratamientoBL11 !
.11! " 
RecuperarTratamiento11" 6
(116 7
idTratamiento117 D
)11D E
;11E F
}22 	
[44 	
	Authorize44	 
(44 
Roles44 
=44 
$str44 1
)441 2
]442 3
public55 
void55 
EliminarTratamiento55 '
(55' (
int55( +
idTratamiento55, 9
)559 :
{66 	
_tratamientoBL77 
.77 
EliminarTratamiento77 .
(77. /
idTratamiento77/ <
)77< =
;77= >
}88 	
[:: 	
	Authorize::	 
(:: 
Roles:: 
=:: 
$str:: 1
)::1 2
]::2 3
public;; 
void;; 
GuardarTratamiento;; &
(;;& '
TratamientoCLS;;' 5
tratamiento;;6 A
);;A B
{<< 	
_tratamientoBL== 
.== 
GuardarTratamiento== -
(==- .
tratamiento==. 9
)==9 :
;==: ;
}>> 	
}AA 
}BB Ñ:
iC:\Users\dalex\OneDrive\Documentos\Code\GestionHospital\GestionHospital\Controllers\PacienteController.cs
	namespace 	
GestionHospital
 
. 
Controllers %
{ 
[		 
	Authorize		 
]		 
public

 

class

 
PacienteController

 #
:

$ %

Controller

& 0
{ 
private 
readonly 

PacienteBL #
_pacienteBL$ /
;/ 0
private 
readonly 
MedicoBL !
	_medicoBL" +
;+ ,
public 
PacienteController !
(! "

PacienteBL" ,

pacienteBL- 7
,7 8
MedicoBL9 A
medicoBLB J
)J K
{ 	
_pacienteBL 
= 

pacienteBL $
;$ %
	_medicoBL 
= 
medicoBL  
;  !
} 	
[ 	
	Authorize	 
( 
Roles 
= 
$str /
)/ 0
]0 1
public 
IActionResult 
Index "
(" #
)# $
{ 	
return 
View 
( 
) 
; 
} 	
[ 	
	Authorize	 
( 
Roles 
= 
$str +
)+ ,
], -
public 
IActionResult 
InfoPersonal )
() *
)* +
{ 	
return 
View 
( 
) 
; 
} 	
[!! 	
	Authorize!!	 
(!! 
Roles!! 
=!! 
$str!! /
)!!/ 0
]!!0 1
public"" 
List"" 
<"" 
PacienteCLS"" 
>""  
ListarPaciente""! /
(""/ 0
)""0 1
{## 	
return$$ 
_pacienteBL$$ 
.$$ 
ListarPaciente$$ -
($$- .
)$$. /
;$$/ 0
}%% 	
[)) 	
	Authorize))	 
()) 
Roles)) 
=)) 
$str)) #
)))# $
]))$ %
public** 
List** 
<** 
PacienteCLS** 
>**  $
ListarPacientesAsignados**! 9
(**9 :
)**: ;
{++ 	
var,, 
	userEmail,, 
=,, 
User,,  
.,,  !
FindFirstValue,,! /
(,,/ 0

ClaimTypes,,0 :
.,,: ;
Email,,; @
),,@ A
;,,A B
var-- 
idDoctor-- 
=-- 
	_medicoBL-- $
.--$ %%
ObtenerIdDoctorDesdeEmail--% >
(--> ?
	userEmail--? H
)--H I
;--I J
return.. 
_pacienteBL.. 
... $
ListarPacientesAsignados.. 7
(..7 8
idDoctor..8 @
)..@ A
;..A B
}// 	
[11 	
	Authorize11	 
(11 
Roles11 
=11 
$str11 (
)11( )
]11) *
public22 
async22 
Task22 
<22 
string22  
>22  !
GuardarPaciente22" 1
(221 2
PacienteCLS222 =
paciente22> F
)22F G
{33 	
await44 
_pacienteBL44 
.44 
GuardarPaciente44 -
(44- .
paciente44. 6
)446 7
;447 8
string55 
password55 
=55 
	AccountBL55 '
.55' (
GenerarContrasena55( 9
(559 :
paciente55: B
.55B C
nombre55C I
,55I J
paciente55K S
.55S T
apellido55T \
,55\ ]
paciente55^ f
.55f g
fechaNacimiento55g v
)55v w
;55w x
return66 
password66 
;66 
}77 	
[99 	
	Authorize99	 
(99 
Roles99 
=99 
$str99 7
)997 8
]998 9
public:: 
PacienteCLS:: 
?:: 
RecuperarPaciente:: -
(::- .
int::. 1

idPaciente::2 <
)::< =
{;; 	
return<< 
_pacienteBL<< 
.<< 
RecuperarPaciente<< 0
(<<0 1

idPaciente<<1 ;
)<<; <
;<<< =
}== 	
[?? 	
	Authorize??	 
(?? 
Roles?? 
=?? 
$str?? *
)??* +
]??+ ,
public@@ 
int@@ #
ObtenerIdPacienteActual@@ *
(@@* +
)@@+ ,
{AA 	
varBB 
	userEmailBB 
=BB 
UserBB  
.BB  !
FindFirstValueBB! /
(BB/ 0

ClaimTypesBB0 :
.BB: ;
EmailBB; @
)BB@ A
;BBA B
returnCC 
_pacienteBLCC 
.CC '
ObtenerIdPacienteDesdeEmailCC :
(CC: ;
	userEmailCC; D
)CCD E
;CCE F
}DD 	
[FF 	
	AuthorizeFF	 
(FF 
RolesFF 
=FF 
$strFF (
)FF( )
]FF) *
publicGG 
asyncGG 
TaskGG 
<GG 
IActionResultGG '
>GG' (
EliminarPacienteGG) 9
(GG9 :
intGG: =

idPacienteGG> H
)GGH I
{HH 	
tryII 
{JJ 
awaitKK 
_pacienteBLKK !
.KK! "
EliminarPacienteKK" 2
(KK2 3

idPacienteKK3 =
)KK= >
;KK> ?
returnLL 
OkLL 
(LL 
$strLL 8
)LL8 9
;LL9 :
}MM 
catchNN 
(NN 
	ExceptionNN 
exNN 
)NN  
{OO 
returnPP 

BadRequestPP !
(PP! "
$strPP" C
+PPD E
exPPF H
.PPH I
MessagePPI P
)PPP Q
;PPQ R
}QQ 
}RR 	
[VV 	
	AuthorizeVV	 
(VV 
RolesVV 
=VV 
$strVV "
)VV" #
]VV# $
publicWW 
asyncWW 
TaskWW 
<WW 
IActionResultWW '
>WW' (/
#CrearCuentasParaPacientesExistentesWW) L
(WWL M
)WWM N
{XX 	
awaitZZ 
_pacienteBLZZ 
.ZZ /
#CrearCuentasParaPacientesExistentesZZ A
(ZZA B
)ZZB C
;ZZC D
return\\ 
RedirectToAction\\ #
(\\# $
$str\\$ +
,\\+ ,
$str\\- 3
)\\3 4
;\\4 5
}]] 	
[__ 	
	Authorize__	 
(__ 
Roles__ 
=__ 
$str__ "
)__" #
]__# $
public`` 
async`` 
Task`` 
<`` 
IActionResult`` '
>``' (7
+DeshabilitarCuentasDePacientesInhabilitados``) T
(``T U
)``U V
{aa 	
awaitcc 
_pacienteBLcc 
.cc 7
+DeshabilitarCuentasDePacientesInhabilitadoscc I
(ccI J
)ccJ K
;ccK L
returndd 
Okdd 
(dd 
$strdd 8
)dd8 9
;dd9 :
}ee 	
}ff 
}gg ‰-
gC:\Users\dalex\OneDrive\Documentos\Code\GestionHospital\GestionHospital\Controllers\MedicoController.cs
	namespace 	
GestionHospital
 
. 
Controllers %
{ 
[		 
	Authorize		 
]		 
public

 

class

 
MedicoController

 !
:

" #

Controller

$ .
{ 
private 
readonly 
MedicoBL !
	_medicoBL" +
;+ ,
public 
MedicoController 
(  
MedicoBL  (
medicoBL) 1
)1 2
{ 	
	_medicoBL 
= 
medicoBL  
;  !
} 	
[ 	
	Authorize	 
( 
Roles 
= 
$str 1
)1 2
]2 3
public 
IActionResult 
Index "
(" #
)# $
{ 	
return 
View 
( 
) 
; 
} 	
[ 	
	Authorize	 
( 
Roles 
= 
$str )
)) *
]* +
public 
List 
< 
	MedicoCLS 
> 
ListarMedico +
(+ ,
), -
{ 	
return 
	_medicoBL 
. 
ListarMedico )
() *
)* +
;+ ,
} 	
[   	
	Authorize  	 
(   
Roles   
=   
$str   *
)  * +
]  + ,
public!! 
IActionResult!! 
InfoPersonal!! )
(!!) *
)!!* +
{"" 	
return## 
View## 
(## 
)## 
;## 
}$$ 	
[&& 	
	Authorize&&	 
(&& 
Roles&& 
=&& 
$str&& "
)&&" #
]&&# $
public'' 
async'' 
Task'' 
<'' 
string''  
>''  !
GuardarMedico''" /
(''/ 0
	MedicoCLS''0 9
medico'': @
)''@ A
{(( 	
await)) 
	_medicoBL)) 
.)) 
GuardarMedico)) )
())) *
medico))* 0
)))0 1
;))1 2
string** 
password** 
=** 
	AccountBL** '
.**' (
GenerarContrasena**( 9
(**9 :
medico**: @
.**@ A
nombre**A G
,**G H
medico**I O
.**O P
apellido**P X
,**X Y
DateTime**Z b
.**b c
Now**c f
)**f g
;**g h
return++ 
password++ 
;++ 
}-- 	
[// 	
	Authorize//	 
(// 
Roles// 
=// 
$str// *
)//* +
]//+ ,
public00 
	MedicoCLS00 
?00 
RecuperarMedico00 )
(00) *
int00* -
idMedico00. 6
)006 7
{11 	
return22 
	_medicoBL22 
.22 
RecuperarMedico22 ,
(22, -
idMedico22- 5
)225 6
;226 7
}33 	
[55 	
	Authorize55	 
(55 
Roles55 
=55 
$str55 *
)55* +
]55+ ,
public66 
int66 !
ObtenerIdMedicoActual66 (
(66( )
)66) *
{77 	
var88 
	userEmail88 
=88 
User88  
.88  !
FindFirstValue88! /
(88/ 0

ClaimTypes880 :
.88: ;
Email88; @
)88@ A
;88A B
return99 
	_medicoBL99 
.99 %
ObtenerIdDoctorDesdeEmail99 6
(996 7
	userEmail997 @
)99@ A
;99A B
}:: 	
[<< 	
	Authorize<<	 
(<< 
Roles<< 
=<< 
$str<< "
)<<" #
]<<# $
public== 
async== 
Task== 
<== 
IActionResult== '
>==' (
EliminarMedico==) 7
(==7 8
int==8 ;
idMedico==< D
)==D E
{>> 	
try?? 
{@@ 
awaitAA 
	_medicoBLAA 
.AA  
EliminarMedicoAA  .
(AA. /
idMedicoAA/ 7
)AA7 8
;AA8 9
returnBB 
OkBB 
(BB 
$strBB :
)BB: ;
;BB; <
}CC 
catchDD 
(DD 
	ExceptionDD 
exDD 
)DD  
{EE 
returnFF 

BadRequestFF !
(FF! "
$strFF" E
+FFF G
exFFH J
.FFJ K
MessageFFK R
)FFR S
;FFS T
}GG 
}HH 	
[JJ 	
	AuthorizeJJ	 
(JJ 
RolesJJ 
=JJ 
$strJJ "
)JJ" #
]JJ# $
publicKK 
asyncKK 
TaskKK 
<KK 
IActionResultKK '
>KK' (-
!CrearCuentasParaMedicosExistentesKK) J
(KKJ K
)KKK L
{LL 	
tryMM 
{NN 
awaitOO 
	_medicoBLOO 
.OO  -
!CrearCuentasParaMedicosExistentesOO  A
(OOA B
)OOB C
;OOC D
returnPP 
OkPP 
(PP 
$strPP @
)PP@ A
;PPA B
}QQ 
catchRR 
(RR 
	ExceptionRR 
exRR 
)RR  
{SS 
returnTT 

BadRequestTT !
(TT! "
$strTT" I
+TTJ K
exTTL N
.TTN O
MessageTTO V
)TTV W
;TTW X
}UU 
}VV 	
}XX 
}YY ’
eC:\Users\dalex\OneDrive\Documentos\Code\GestionHospital\GestionHospital\Controllers\HomeController.cs
	namespace 	
GestionHospital
 
. 
Controllers %
{ 
public 

class 
HomeController 
:  !

Controller" ,
{ 
private		 
readonly		 
ILogger		  
<		  !
HomeController		! /
>		/ 0
_logger		1 8
;		8 9
public 
HomeController 
( 
ILogger %
<% &
HomeController& 4
>4 5
logger6 <
)< =
{ 	
_logger 
= 
logger 
; 
} 	
public 
IActionResult 
Index "
(" #
)# $
{ 	
return 
View 
( 
) 
; 
} 	
public 
IActionResult 
Privacy $
($ %
)% &
{ 	
return 
View 
( 
) 
; 
} 	
public 
IActionResult 
Info !
(! "
)" #
{ 	
return 
View 
( 
) 
; 
} 	
[ 	
ResponseCache	 
( 
Duration 
=  !
$num" #
,# $
Location% -
=. /!
ResponseCacheLocation0 E
.E F
NoneF J
,J K
NoStoreL S
=T U
trueV Z
)Z [
][ \
public   
IActionResult   
Error   "
(  " #
)  # $
{!! 	
return"" 
View"" 
("" 
new"" 
ErrorViewModel"" *
{""+ ,
	RequestId""- 6
=""7 8
Activity""9 A
.""A B
Current""B I
?""I J
.""J K
Id""K M
??""N P
HttpContext""Q \
.""\ ]
TraceIdentifier""] l
}""m n
)""n o
;""o p
}## 	
}$$ 
}%% ¨#
lC:\Users\dalex\OneDrive\Documentos\Code\GestionHospital\GestionHospital\Controllers\FacturacionController.cs
	namespace 	
GestionHospital
 
. 
Controllers %
{		 
[

 
	Authorize

 
]

 
public 

class !
FacturacionController &
:' (

Controller) 3
{ 
private 
readonly 
FacturacionBL &
_facturacionBL' 5
;5 6
private 
readonly 

PacienteBL #
_pacienteBL$ /
;/ 0
public !
FacturacionController $
($ %
FacturacionBL% 2
facturacionBL3 @
,@ A

PacienteBLB L

pacienteBLM W
)W X
{ 	
_facturacionBL 
= 
facturacionBL *
;* +
_pacienteBL 
= 

pacienteBL $
;$ %
} 	
[ 	
	Authorize	 
( 
Roles 
= 
$str 2
)2 3
]3 4
public 
IActionResult 
Index "
(" #
)# $
{ 	
return 
View 
( 
) 
; 
} 	
[ 	
	Authorize	 
( 
Roles 
= 
$str )
)) *
]* +
public 
List 
< 
FacturacionViewCLS &
>& '
ListarFacturacion( 9
(9 :
): ;
{ 	
List   
<   
FacturacionViewCLS   #
>  # $
facturacion  % 0
=  1 2
_facturacionBL  3 A
.  A B
ListarFacturacion  B S
(  S T
)  T U
;  U V
return!! 
facturacion!! 
;!! 
}"" 	
[$$ 	
	Authorize$$	 
($$ 
Roles$$ 
=$$ 
$str$$ +
)$$+ ,
]$$, -
public%% 
List%% 
<%% 
FacturacionCLS%% "
>%%" #%
ListarFacturacionPaciente%%$ =
(%%= >
)%%> ?
{&& 	
var'' 
	userEmail'' 
='' 
User''  
.''  !
FindFirstValue''! /
(''/ 0

ClaimTypes''0 :
.'': ;
Email''; @
)''@ A
;''A B
var(( 

idPaciente(( 
=(( 
_pacienteBL(( (
.((( )'
ObtenerIdPacienteDesdeEmail(() D
(((D E
	userEmail((E N
)((N O
;((O P
List)) 
<)) 
FacturacionCLS)) 
>))  
facturasPaciente))! 1
=))2 3
_facturacionBL))4 B
.))B C%
ListarFacturacionPaciente))C \
())\ ]

idPaciente))] g
)))g h
;))h i
return** 
facturasPaciente** #
;**# $
}++ 	
[-- 	
	Authorize--	 
(-- 
Roles-- 
=-- 
$str-- )
)--) *
]--* +
public.. 
FacturacionCLS.. 
?..  
RecuperarFacturacion.. 3
(..3 4
int..4 7
idFacturacion..8 E
)..E F
{// 	
return00 
_facturacionBL00 !
.00! " 
RecuperarFacturacion00" 6
(006 7
idFacturacion007 D
)00D E
;00E F
}11 	
[33 	
	Authorize33	 
(33 
Roles33 
=33 
$str33 )
)33) *
]33* +
public44 
void44 
EliminarFacturacion44 '
(44' (
int44( +
idFacturacion44, 9
)449 :
{55 	
_facturacionBL66 
.66 
EliminarFacturacion66 .
(66. /
idFacturacion66/ <
)66< =
;66= >
}77 	
[99 	
	Authorize99	 
(99 
Roles99 
=99 
$str99 )
)99) *
]99* +
public:: 
void:: 
GuardarFacturacion:: &
(::& '
FacturacionCLS::' 5
facturacion::6 A
)::A B
{;; 	
_facturacionBL<< 
.<< 
GuardarFacturacion<< -
(<<- .
facturacion<<. 9
)<<9 :
;<<: ;
}== 	
}>> 
}?? é+
eC:\Users\dalex\OneDrive\Documentos\Code\GestionHospital\GestionHospital\Controllers\CitaController.cs
	namespace 	
GestionHospital
 
. 
Controllers %
{ 
[		 
	Authorize		 
]		 
public

 

class

 
CitaController

 
:

  !

Controller

" ,
{ 
private 
readonly 
CitaBL 
_citaBL  '
;' (
private 
readonly 
MedicoBL !
	_medicoBL" +
;+ ,
private 
readonly 

PacienteBL #
_pacienteBL$ /
;/ 0
public 
CitaController 
( 
CitaBL $
citaBL% +
,+ ,
MedicoBL, 4
medicoBL5 =
,= >

PacienteBL? I

pacienteBLJ T
)T U
{ 	
_citaBL 
= 
citaBL 
; 
	_medicoBL 
= 
medicoBL  
;  !
_pacienteBL 
= 

pacienteBL $
;$ %
} 	
[ 	
	Authorize	 
( 
Roles 
= 
$str :
): ;
]; <
public 
IActionResult 
Index "
(" #
)# $
{ 	
return 
View 
( 
) 
; 
} 	
[ 	
	Authorize	 
( 
Roles 
= 
$str )
)) *
]* +
public 
List 
< 
CitaViewCLS 
>  
ListarCitas! ,
(, -
)- .
{ 	
List 
< 
CitaViewCLS 
> 
citas #
=$ %
_citaBL& -
.- .

ListarCita. 8
(8 9
)9 :
;: ;
return 
citas 
; 
}   	
["" 	
	Authorize""	 
("" 
Roles"" 
="" 
$str"" )
)"") *
]""* +
public## 
List## 
<## 
CitaViewCLS## 
>##  
ListarCitasMedico##! 2
(##2 3
)##3 4
{$$ 	
var%% 
	userEmail%% 
=%% 
User%%  
.%%  !
FindFirstValue%%! /
(%%/ 0

ClaimTypes%%0 :
.%%: ;
Email%%; @
)%%@ A
;%%A B
var&& 
idMedico&& 
=&& 
	_medicoBL&& $
.&&$ %%
ObtenerIdDoctorDesdeEmail&&% >
(&&> ?
	userEmail&&? H
)&&H I
;&&I J
List'' 
<'' 
CitaViewCLS'' 
>'' 
citas'' #
=''$ %
_citaBL''& -
.''- .
ListarCitasMedico''. ?
(''? @
idMedico''@ H
)''H I
;''I J
return(( 
citas(( 
;(( 
})) 	
[++ 	
	Authorize++	 
(++ 
Roles++ 
=++ 
$str++ *
)++* +
]+++ ,
public,, 
List,, 
<,, 
CitaViewCLS,, 
>,,  
ListarCitasPaciente,,! 4
(,,4 5
),,5 6
{-- 	
var.. 
	userEmail.. 
=.. 
User..  
...  !
FindFirstValue..! /
(../ 0

ClaimTypes..0 :
...: ;
Email..; @
)..@ A
;..A B
var// 

idPaciente// 
=// 
_pacienteBL// (
.//( )'
ObtenerIdPacienteDesdeEmail//) D
(//D E
	userEmail//E N
)//N O
;//O P
List00 
<00 
CitaViewCLS00 
>00 
citas00 #
=00$ %
_citaBL00& -
.00- .
ListarCitasPaciente00. A
(00A B

idPaciente00B L
)00L M
;00M N
return11 
citas11 
;11 
}22 	
[55 	
	Authorize55	 
(55 
Roles55 
=55 
$str55 1
)551 2
]552 3
public66 
void66 
GuardarCita66 
(66  
CitaCLS66  '
cita66( ,
)66, -
{77 	
_citaBL88 
.88 
GuardarCita88 
(88  
cita88  $
)88$ %
;88% &
}99 	
[;; 	
	Authorize;;	 
(;; 
Roles;; 
=;; 
$str;; 1
);;1 2
];;2 3
public<< 
CitaCLS<< 
?<< 
RecuperarCita<< %
(<<% &
int<<& )
idCita<<* 0
)<<0 1
{== 	
return>> 
_citaBL>> 
.>> 
RecuperarCita>> (
(>>( )
idCita>>) /
)>>/ 0
;>>0 1
}?? 	
[AA 	
	AuthorizeAA	 
(AA 
RolesAA 
=AA 
$strAA 1
)AA1 2
]AA2 3
publicBB 
voidBB 
EliminarCitaBB  
(BB  !
intBB! $
idCitaBB% +
)BB+ ,
{CC 	
_citaBLDD 
.DD 
EliminarCitaDD  
(DD  !
idCitaDD! '
)DD' (
;DD( )
}EE 	
}GG 
}HH ŒA
hC:\Users\dalex\OneDrive\Documentos\Code\GestionHospital\GestionHospital\Controllers\AccountController.cs
	namespace 	
GestionHospital
 
. 
Controllers %
{ 
public		 

class		 
AccountController		 "
:		# $

Controller		% /
{

 
private 
readonly 
RoleManager $
<$ %
IdentityRole% 1
>1 2
_roleManager3 ?
;? @
private 
readonly 
SignInManager &
<& '
IdentityUser' 3
>3 4
_signInManager5 C
;C D
private 
readonly 
UserManager $
<$ %
IdentityUser% 1
>1 2
_userManager3 ?
;? @
private 
readonly 
PacienteDAL $
_pacienteDAL% 1
;1 2
public 
AccountController  
(  !
SignInManager! .
<. /
IdentityUser/ ;
>; <
signInManager= J
,J K
UserManagerL W
<W X
IdentityUserX d
>d e
userManagerf q
,q r
RoleManagers ~
<~ 
IdentityRole	 ã
>
ã å
roleManager
ç ò
,
ò ô
PacienteDAL
ö •
pacienteDAL
¶ ±
)
± ≤
{ 	
_signInManager 
= 
signInManager *
;* +
_userManager 
= 
userManager &
;& '
_roleManager 
= 
roleManager &
;& '
_pacienteDAL 
= 
pacienteDAL &
;& '
} 	
[ 	
HttpGet	 
] 
public 
IActionResult 
Login "
(" #
)# $
{ 	
return 
View 
( 
) 
; 
} 	
[ 	
HttpGet	 
] 
public 
IActionResult 
Register %
(% &
)& '
{   	
return!! 
View!! 
(!! 
)!! 
;!! 
}"" 	
[$$ 	
HttpPost$$	 
]$$ 
public%% 
async%% 
Task%% 
<%% 
IActionResult%% '
>%%' (
Login%%) .
(%%. /
LoginViewModel%%/ =
model%%> C
)%%C D
{&& 	
if'' 
('' 

ModelState'' 
.'' 
IsValid'' "
)''" #
{(( 
var)) 
result)) 
=)) 
await)) "
_signInManager))# 1
.))1 2
PasswordSignInAsync))2 E
())E F
model))F K
.))K L
Email))L Q
,))Q R
model))S X
.))X Y
Password))Y a
,))a b
model))c h
.))h i

RememberMe))i s
,))s t
lockoutOnFailure	))u Ö
:
))Ö Ü
false
))á å
)
))å ç
;
))ç é
if** 
(** 
result** 
.** 
	Succeeded** $
)**$ %
{++ 
return,, 
RedirectToAction,, +
(,,+ ,
$str,,, 3
,,,3 4
$str,,5 ;
),,; <
;,,< =
}-- 

ModelState.. 
... 
AddModelError.. (
(..( )
string..) /
.../ 0
Empty..0 5
,..5 6
$str..7 O
)..O P
;..P Q
}// 
return00 
View00 
(00 
model00 
)00 
;00 
}11 	
[33 	
HttpPost33	 
]33 
[44 	$
ValidateAntiForgeryToken44	 !
]44! "
public55 
async55 
Task55 
<55 
IActionResult55 '
>55' (
Logout55) /
(55/ 0
)550 1
{66 	
await77 
_signInManager77  
.77  !
SignOutAsync77! -
(77- .
)77. /
;77/ 0
return88 
RedirectToAction88 #
(88# $
$str88$ +
,88+ ,
$str88- 3
)883 4
;884 5
}99 	
[:: 	
HttpPost::	 
]:: 
public;; 
async;; 
Task;; 
<;; 
IActionResult;; '
>;;' (
Register;;) 1
(;;1 2
RegisterViewModel;;2 C
model;;D I
);;I J
{<< 	
if== 
(== 

ModelState== 
.== 
IsValid== "
)==" #
{>> 
var?? 
user?? 
=?? 
new?? 
IdentityUser?? +
{??, -
UserName??. 6
=??7 8
model??9 >
.??> ?
Email??? D
,??D E
Email??F K
=??L M
model??N S
.??S T
Email??T Y
}??Z [
;??[ \
var@@ 
result@@ 
=@@ 
await@@ "
_userManager@@# /
.@@/ 0
CreateAsync@@0 ;
(@@; <
user@@< @
,@@@ A
model@@B G
.@@G H
Password@@H P
)@@P Q
;@@Q R
ifAA 
(AA 
resultAA 
.AA 
	SucceededAA $
)AA$ %
{BB 
ifDD 
(DD 
!DD 
awaitDD 
_roleManagerDD +
.DD+ ,
RoleExistsAsyncDD, ;
(DD; <
$strDD< E
)DDE F
)DDF G
{EE 
awaitFF 
_roleManagerFF *
.FF* +
CreateAsyncFF+ 6
(FF6 7
newFF7 :
IdentityRoleFF; G
(FFG H
$strFFH Q
)FFQ R
)FFR S
;FFS T
}GG 
awaitJJ 
_userManagerJJ &
.JJ& '
AddToRoleAsyncJJ' 5
(JJ5 6
userJJ6 :
,JJ: ;
$strJJ< E
)JJE F
;JJF G
varMM 
pacienteMM  
=MM! "
newMM# &
PacienteCLSMM' 2
{NN 
nombreOO 
=OO  
modelOO! &
.OO& '
NombreOO' -
,OO- .
apellidoPP  
=PP! "
modelPP# (
.PP( )
ApellidoPP) 1
,PP1 2
fechaNacimientoQQ '
=QQ( )
modelQQ* /
.QQ/ 0
FechaNacimientoQQ0 ?
,QQ? @
telefonoRR  
=RR! "
modelRR# (
.RR( )
TelefonoRR) 1
,RR1 2
emailSS 
=SS 
modelSS  %
.SS% &
EmailSS& +
,SS+ ,
	direccionTT !
=TT" #
modelTT$ )
.TT) *
	DireccionTT* 3
,TT3 4
BHABILITADOUU #
=UU$ %
$numUU& '
}VV 
;VV 
_pacienteDALWW  
.WW  !
GuardarPacienteWW! 0
(WW0 1
pacienteWW1 9
)WW9 :
;WW: ;
awaitYY 
_signInManagerYY (
.YY( )
SignInAsyncYY) 4
(YY4 5
userYY5 9
,YY9 :
isPersistentYY; G
:YYG H
falseYYI N
)YYN O
;YYO P
returnZZ 
RedirectToActionZZ +
(ZZ+ ,
$strZZ, 3
,ZZ3 4
$strZZ5 ;
)ZZ; <
;ZZ< =
}[[ 
foreach\\ 
(\\ 
var\\ 
error\\ "
in\\# %
result\\& ,
.\\, -
Errors\\- 3
)\\3 4
{]] 

ModelState^^ 
.^^ 
AddModelError^^ ,
(^^, -
string^^- 3
.^^3 4
Empty^^4 9
,^^9 :
error^^; @
.^^@ A
Description^^A L
)^^L M
;^^M N
}__ 
}`` 
returnaa 
Viewaa 
(aa 
modelaa 
)aa 
;aa 
}bb 	
[dd 	
HttpGetdd	 
]dd 
publicee 
IActionResultee 
AccessDeniedee )
(ee) *
)ee* +
{ff 	
returngg 
Viewgg 
(gg 
)gg 
;gg 
}hh 	
}ll 
}mm 